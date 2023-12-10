using BosonHiggsApi.BL.Enums;

namespace BosonHiggsApi.DL.Entities
{
    public class Level
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public string? Link { get; set; }

        public string? Hint { get; set; }

        public string? Token { get; set; }

        public LevelType Type { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public DateTime? UpdatedDateTime { get; set;}

        public string? NextLevelId { get; set; }

        public Level? NextLevel { get; set; }

        public ICollection<UserLevels> UserLevels { get; set; }
    }
}
