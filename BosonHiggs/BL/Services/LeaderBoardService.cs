using BosonHiggsApi.BL.Enums;
using BosonHiggsApi.BL.Helpers;
using BosonHiggsApi.BL.Models;
using BosonHiggsApi.DL;
using Microsoft.EntityFrameworkCore;

namespace BosonHiggsApi.BL.Services
{
    public class LeaderBoardService
    {
        private readonly ApplicationDbContext _context;
        public LeaderBoardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<LeaderBoardModel> List()
        {
            var result = new List<LeaderModel>();
            var users = await _context.Users
                .Where(x => x.Token != AppConstants.AdminToken)
                .Include(x => x.UserLevels)!
                .ThenInclude(x => x.Level)
                .ToListAsync();

            foreach (var user in users)
            {
                var totalSpentTime = 0;
                if (user.UserLevels == null) continue;
                
                var last = user.UserLevels.FirstOrDefault();
                if (last == null)
                {
                    result.Add(new LeaderModel
                    {
                        NickName = user.NickName,
                        LevelType = 0,
                        TotalSpentTime = 0,
                        UsedHintsCount = 0,
                        UsedNextLevelHintsCount = 0,
                        LastLevelStartedDateTime = null
                    });

                    continue;
                }

                foreach (var userLevel in user.UserLevels.OrderBy(x => x.CreatedDateTime))
                {
                    if (userLevel.Level.Type == LevelType.First)
                    {
                        last = userLevel;
                        continue;
                    }
                    
                    totalSpentTime += userLevel.CreatedDateTime.Minute - last.CreatedDateTime.Minute;
                }

                if (!user.UserLevels.Any())
                {
                    result.Add(new LeaderModel
                    {
                        NickName = user.NickName,
                        LevelType = 0,
                        TotalSpentTime = 0,
                        UsedHintsCount = 0,
                        UsedNextLevelHintsCount = 0,
                        LastLevelStartedDateTime = null
                    });
                }
                else
                {
                    result.Add(new LeaderModel
                    {
                        NickName = user.NickName,
                        LevelType = user.UserLevels.Max(x => x.Level.Type),
                        TotalSpentTime = totalSpentTime,
                        UsedHintsCount = user.UserLevels.Count(x => x.UsedHint == true),
                        UsedNextLevelHintsCount = user.UserLevels.Count(x => x.UsedNextLevelHint == true),
                        LastLevelStartedDateTime = last.CreatedDateTime
                    });
                }
            }

            var ind = 1;
            var leaders = result
                .OrderByDescending(x => x.LevelType)
                .ThenBy(x => x.LastLevelStartedDateTime) //TODO: configure-out
                .ThenBy(x => x.UsedHintsCount)
                .ThenBy(x => x.UsedNextLevelHintsCount)
                .ToList();
            leaders.ForEach(x => x.Id = ind++);

            return new LeaderBoardModel()
            {
                TotalUserCount = result.Count,
                Leaders = leaders
            };
        }
    }
}