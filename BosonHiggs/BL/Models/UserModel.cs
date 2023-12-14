using BosonHiggsApi.BL.Enums;

namespace BosonHiggsApi.BL.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        public string NickName { get; set; }
        public LevelType LevelType { get; set; }
        public int TotalSpentTime{ get; set; }
        public DateTime? LastLevelStartedDateTime { get; set; }
        public int UsedHintsCount { get; set; }
        public int UsedNextLevelHintsCount { get; set; }
    }
}
