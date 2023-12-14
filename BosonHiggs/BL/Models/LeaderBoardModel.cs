using BosonHiggsApi.BL.Enums;

namespace BosonHiggsApi.BL.Models
{
    public class LeaderBoardModel
    {
        public int TotalUserCount { get; set; }

        public IList<LeaderModel> Leaders { get; set; }
    }

    public class LeaderModel
    {
        public int Id { get; set; }
        public string NickName { get; set; }
        public LevelType LevelType { get; set; }
        public int TotalSpentTime { get; set; }
        public DateTime? LastLevelStartedDateTime { get; set; }
        public int UsedHintsCount { get; set; }
        public int UsedNextLevelHintsCount { get; set; }
    }
}
