using BosonHiggsApi.BL.Enums;
using BosonHiggsApi.BL.Exceptions;
using BosonHiggsApi.BL.Helpers;
using BosonHiggsApi.BL.Models;
using BosonHiggsApi.BL.Models.EmailContents;
using BosonHiggsApi.BL.Services.Email;
using BosonHiggsApi.DL;
using BosonHiggsApi.DL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BosonHiggsApi.BL.Services
{
    public class AccountService
    {
        private readonly ApplicationDbContext _context;
        private readonly AggregateEmailSender _emailSender;
        public AccountService(ApplicationDbContext context, AggregateEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        public async Task<string> RegisterUserAsync(RegisterModel model, string ip)
        {
            if (model.NickName.Contains("Admin"))
                throw new BadRequestException($"User's nickname can't contain \'Admin\'");
            if (_context.Users.Any(x => x.Email == model.Email))
                throw new BadRequestException($"User with the same email - \'{model.Email}\' already exist");
            if (_context.Users.Any(x => x.NickName == model.NickName))
                throw new BadRequestException($"User with the same nickname - \'{model.NickName}\' already exist");
            
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Token = RandomString.AlphaNumeric(64),
                NickName = model.NickName,
                Email = model.Email,
                IpAddress = ip,
                CreatedDateTime = DateTime.Now,
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            try
            {
                await _emailSender.SendAsync(new SendTokenEmailContent(user.NickName, user.Token), user.Email); //TODO: check
            }
            catch (Exception ex)
            {

            }

            return user.Token;
        }

        public async Task<LeaderModel> AboutMe(string userToken)
        {
            var user = await _context.Users
                .Include(x => x.UserLevels)
                .FirstOrDefaultAsync(x => x.Token == userToken);
            if (user == null)
                throw new BadRequestException($"User with the token - \'{userToken}\' doesn't exist");

            var totalSpentTime = 0;
            var last = user.UserLevels.FirstOrDefault();
            foreach (var userLevel in user.UserLevels.OrderBy(x => x.CreatedDateTime))
            {
                if (userLevel.Level.Type == LevelType.First)
                {
                    last = userLevel;
                    continue;
                }

                totalSpentTime += userLevel.CreatedDateTime.Minute - last.CreatedDateTime.Minute;
            }

            return new LeaderModel
            {
                NickName = user.NickName,
                LevelType = user.UserLevels.Max(x => x.Level.Type),
                TotalSpentTime = totalSpentTime,
                UsedHintsCount = user.UserLevels.Count(x => x.UsedHint == true),
                UsedNextLevelHintsCount = user.UserLevels.Count(x => x.UsedNextLevelHint == true),
                LastLevelStartedDateTime = last.CreatedDateTime
            };
        }
    }
}
