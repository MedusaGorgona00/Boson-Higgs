using BosonHiggsApi.BL.Enums;

namespace BosonHiggsApi.BL.Models
{
    public class LeaderModel
    {
        public string NickName { get; set; }
        public LevelType LevelType { get; set; }
        public int TotalSpentTime{ get; set; }
        public int UsedHintsCount { get; set; }
        public int UsedNextLevelHintsCount { get; set; }
    }
}
