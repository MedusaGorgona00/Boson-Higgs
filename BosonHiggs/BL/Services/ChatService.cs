using BosonHiggsApi.BL.Enums;
using BosonHiggsApi.BL.Exceptions;
using BosonHiggsApi.BL.Helpers;
using BosonHiggsApi.BL.Models;
using BosonHiggsApi.DL;
using BosonHiggsApi.DL.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BosonHiggsApi.BL.Services
{
    public class ChatService
    {
        private readonly IHubContext<ChatHub> _chatHub;
        private readonly ApplicationDbContext _context;

        public ChatService(ApplicationDbContext context, IHubContext<ChatHub> hub)
        {
            _context = context;
            _chatHub = hub;
        }

        public async Task SendMessage(MessageModel.In model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Token == model.UserToken);
            if (user == null)
                throw new BadRequestException($"User with token {model.UserToken} doesn't exist");

            var levels = _context.Levels
                .Select(x => new
                {
                    x.Token,
                    x.Link,
                    x.Id
                })
                .ToList();

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(levels);
            if (json.Contains(model.Text) || model.Text.Contains(AppConstants.LastLevelPassword))
                throw new BadRequestException("Mustn't send hints, level tokens, level ids as message");
            if (AppConstants.BadWords.Contains(model.Text.ToLower()))
                throw new BadRequestException("Mustn't use an obscene words in message");

            var message = new Message()
            {
                Text = model.Text,
                UserId = user.Id,
                IsDeleted = false,
                CreatedDateTime = DateTime.Now,
            };

            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();

            var chatModel = new MessageModel.Out()
            {
                Text = message.Text,
                NickName = user.NickName,
                IsDeleted = message.IsDeleted,
                CreatedDateTime = message.CreatedDateTime
            };

            await _chatHub.Clients.All.SendAsync("new-message", chatModel);
        }

        public async Task<IList<MessageModel.Out>> List(string userToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Token == userToken);
            if (user == null)
                throw new BadRequestException($"User with token {userToken} doesn't exist");

            var messages = _context.Messages
                .Include(x => x.User)
                .Select(x => new MessageModel.Out()
                {
                    Id = x.Id,
                    Text = x.Text,
                    NickName = user.NickName,
                    IsDeleted = x.IsDeleted,
                    CreatedDateTime = x.CreatedDateTime
                })
                .ToList();

            return messages;
        }

        public async Task Remove(string userToken, int messageId)
        {
            if (userToken != AppConstants.AdminToken)
                throw new BadRequestException($"User with token {userToken} doesn't have access");

            var message = await _context.Messages.FirstOrDefaultAsync(X => X.Id == messageId);
            if (message == null)
                throw new NotFoundException($"Message with id {messageId} doesn't exist");

            message.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
    }
}