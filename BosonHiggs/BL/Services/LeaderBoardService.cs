using BosonHiggsApi.BL.Enums;
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

        public async Task<IList<LeaderModel>> List()
        {
            var result = new List<LeaderModel>();
            var users = await _context.Users
                .Include(x => x.UserLevels)
                .ThenInclude(x => x.Level)
                .ToListAsync();

            foreach (var user in users)
            {
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

                if (!user.UserLevels.Any())
                {
                    result.Add(new LeaderModel
                    {
                        NickName = user.NickName,
                        LevelType = 0,
                        TotalSpentTime = 0,
                        UsedHintsCount = 0,
                        UsedNextLevelHintsCount = 0,
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
                    });
                }
            }


            return result.OrderByDescending(x => x.LevelType)
                .ThenBy(x => x.TotalSpentTime) //TODO: who pass first or total time?
                .ThenBy(x => x.UsedHintsCount)
                .ThenBy(x => x.UsedNextLevelHintsCount)
                .ToList();
        }
    }
}