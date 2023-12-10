using System.ComponentModel.DataAnnotations;

namespace BosonHiggsApi.DL.Entities
{
    public class UserLevels
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        
        public User? User { get; set; }

        public string LevelId { get; set; }

        public Level? Level { get; set; }

        public bool? UsedHint { get; set; }

        public DateTime? UsedHintDateTime { get; set; }

        public bool? UsedNextLevelHint { get; set; }

        public DateTime? UsedNextLevelHintDateTime { get; set; }

        public DateTime CreatedDateTime { get; set; }
    }
}
