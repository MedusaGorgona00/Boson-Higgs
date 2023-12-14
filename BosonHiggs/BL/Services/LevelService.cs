using BosonHiggsApi.BL.Enums;
using BosonHiggsApi.BL.Exceptions;
using BosonHiggsApi.BL.Helpers;
using BosonHiggsApi.BL.Models;
using BosonHiggsApi.DL;
using BosonHiggsApi.DL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BosonHiggsApi.BL.Services
{
    public class LevelService
    {
        private readonly ApplicationDbContext _context;
        public LevelService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Update(LevelModel.Update model)
        {
            //var user = await _context.Users.FirstOrDefaultAsync(x => x.Token == model.UserToken);
            //if (user == null)
            //    throw new NotFoundException("User with given token not found");
            //if (user.NickName != "Admin")
            //    throw new ForbiddenException("Only admin has access");

            var level = await _context.Levels
                .FirstOrDefaultAsync(x => x.Id == model.Id);
            if (level == null)
                throw new NotFoundException($"Level with given identifier not found");
            
            level.Name = model.Name;
            level.Description = model.Description;
            level.Token = model.Token;
            level.NextLevelId = model.NextLevelId;
            level.Link = model.Link;
            level.Hint = model.Hint;
            level.Type = model.Type;
            level.UpdatedDateTime = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        public async Task<LevelModel.GetByUser> GetFirstLevel(string userToken)
        {
            var level = await _context.Levels
                .Include(x => x.UserLevels)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Type == LevelType.First);
            if (level == null)
                throw new NotFoundException($"Level with given identifier not found");

            if (!level.UserLevels.Any(x => x.User.Token == userToken))
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Token == userToken);
                if (user == null)
                    throw new NotFoundException($"User with given token not found");

                var userLevel = new UserLevels()
                {
                    LevelId = level.Id,
                    UserId = user.Id,
                    UsedHint = false,
                    UsedNextLevelHint = false,
                    CreatedDateTime = DateTime.Now
                };

                _context.UserLevels.Add(userLevel);
                await _context.SaveChangesAsync();
            }

            return new LevelModel.GetByUser()
            {
                Id = level.Id,
                Description = level.Description,
                Name = level.Name,
                Link = level.Link,
                Token = level.Token,
                Type = level.Type
            };
        }

        public async Task<LevelModel.GetByUser> GetByToken(string token, string userToken)
        {
            var level = await _context.Levels
                .Include(x => x.UserLevels)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Token == token);
            if (level == null)
                throw new NotFoundException($"Level with given identifier not found");

            if (!level.UserLevels.Any(x => x.User.Token == userToken))
            {
                var user = await _context.Users
                    .Include(x => x.UserLevels)
                    .ThenInclude(x => x.Level)
                    .FirstOrDefaultAsync(x => x.Token == userToken);
                if (user == null)
                    throw new NotFoundException($"User with given token not found");

                var levelsBefore = Enumerable
                    .Range(1, (int)level.Type - 1)
                    .ToList();
                var userLevels = user.UserLevels
                    .Select(x => (int)x.Level.Type)
                    .ToList();
                if (levelsBefore.Except(userLevels).Any())
                    throw new BadRequestException($"User must pass all previous levels before apply to {(int)level.Type}-level");

                var userLevel = new UserLevels()
                {
                    LevelId = level.Id,
                    UserId = user.Id,
                    UsedHint = false,
                    UsedNextLevelHint = false,
                    CreatedDateTime = DateTime.Now
                };

                _context.UserLevels.Add(userLevel);
                await _context.SaveChangesAsync();
            }

            return new LevelModel.GetByUser()
            {
                Id = level.Id,
                Description = level.Description,
                Name = level.Name,
                Link = level.Link,
                Token = level.Token,
                Type = level.Type
            };
        }

        public async Task<string> GetHint(string id, string userToken)
        {
            var level = await _context.Levels
                .Include(x => x.NextLevel)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (level == null)
                throw new NotFoundException($"Level with given identifier not found");

            var userLevel = await _context.UserLevels
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.User.Token == userToken && x.LevelId == id);
            if (userLevel == null)
                throw new NotFoundException($"Level doesn't have such user");
            
            if (userLevel.CreatedDateTime.AddMinutes(AppConstants.ActivateHintInMinutes) > DateTime.Now)
            {
                var activateTime = (userLevel.CreatedDateTime.AddMinutes(AppConstants.ActivateHintInMinutes) - DateTime.Now).Minutes;
                throw new BadRequestException($"Hint is disable now. Please wait \'{activateTime}\' minutes to activate hint");
            }

            userLevel.UsedHint = true;
            userLevel.UsedHintDateTime = DateTime.Now;

            await _context.SaveChangesAsync();

            return level.Hint;
        }

        public async Task<string?> GetNextLevelToken(string id, string userToken)
        {
            var level = await _context.Levels
                .Include(x => x.NextLevel)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (level == null)
                throw new NotFoundException($"Level with given identifier not found");

            var userLevel = await _context.UserLevels
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.User.Token == userToken && x.LevelId == id); 
            if (userLevel == null)
                throw new NotFoundException($"Level doesn't have such user");
            if (userLevel.CreatedDateTime.AddMinutes(AppConstants.ActivateNextLevelHintInMinutes) < DateTime.Now)
            {
                var activateTime = DateTime.Now.Date - userLevel.CreatedDateTime.AddMinutes(AppConstants.ActivateNextLevelHintInMinutes).Date;
                throw new BadRequestException($"Hint is disable now. Please wait \'{activateTime.Minutes}\' before activate hint");
            }

            userLevel.UsedNextLevelHint = true;
            userLevel.UsedNextLevelHintDateTime = DateTime.Now;

            await _context.SaveChangesAsync();

            if (level.Type == LevelType.Seventh)
                return AppConstants.LastLevelPassword;

            return level?.NextLevel?.Token;
        }

        public async Task<IList<LevelModel.GetByAdmin>> List()
        {
            //var user = await _context.Users.FirstOrDefaultAsync(x => x.Token == userToken);
            //if (user == null)
            //    throw new NotFoundException("User with given token not found");
            //if (user.NickName != "Admin")
            //    throw new ForbiddenException("Only admin has access");

            return await _context.Levels.Select(x => new LevelModel.GetByAdmin()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        Hint = x.Hint,
                        Link = x.Link,
                        NextLevelId = x.NextLevelId,
                        Token = x.Token,
                        Type = x.Type,
                    })
                .ToListAsync();
        }
    }
}