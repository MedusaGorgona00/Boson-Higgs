using BosonHiggsApi.BL.Enums;
using BosonHiggsApi.BL.Exceptions;
using BosonHiggsApi.BL.Helpers;
using BosonHiggsApi.BL.Models;
using BosonHiggsApi.BL.Models.EmailContents;
using BosonHiggsApi.BL.Services.Email;
using BosonHiggsApi.DL;
using BosonHiggsApi.DL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BosonHiggsApi.BL.Services
{
    public class BruteforceService
    {
        private readonly ApplicationDbContext _context;
        public BruteforceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> Login(BruteforceModel model)
        {
            if (model.Email != AppConstants.LastLevelLogin)
                throw new NotFoundException($"Incorrect email");
            if (model.Password != AppConstants.LastLevelPassword)
                throw new BadRequestException($"Incorrect password");

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Token == model.UserToken);

            if (user == null)
                throw new BadRequestException($"User with token - '{model.UserToken}\' doesn't exist");

            var level = await _context.Levels.FirstOrDefaultAsync(x => x.Type == LevelType.Finish);
            var userLevel = new UserLevels()
            {
                UserId = user.Id,
                LevelId = level.Id,
                CreatedDateTime = DateTime.Now,
            };

            await _context.UserLevels.AddAsync(userLevel);
            await _context.SaveChangesAsync();
            

            return "https://docs.google.com/document/d/1XKKQLW8ccwL-L2Oeg4jxQqwu4XNdcj-RX3-TEBuzSv4/edit?usp=sharing";
        }
    }
}