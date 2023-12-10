using System.ComponentModel.DataAnnotations;

namespace BosonHiggsApi.DL.Entities
{
    public class Message
    {
        public int Id { get; set; }

        [MaxLength(256)]
        public string Text { get; set; }

        public bool IsDeleted { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public DateTime CreatedDateTime { get; set; }
    }
}
