using BosonHiggsApi.BL.Enums;
using System.ComponentModel.DataAnnotations;

namespace BosonHiggsApi.BL.Models
{
    public class LevelModel
    {
        public class Base
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public string? Description { get; set; }

            public string? Link { get; set; }

            public LevelType Type { get; set; }
        }

        public class Update : Base
        {
            public string? Hint { get; set; }

            public string? Token { get; set; }

            public string? NextLevelId { get; set; }
        }

        public class GetByUser : Base
        {
            public string? Token { get; set; }
        }

        public class GetByAdmin : Base
        {
            public string? Hint { get; set; }

            public string? Token { get; set; }

            public string? NextLevelId { get; set; }
        }
    }
}
