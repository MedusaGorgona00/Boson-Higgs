using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace BosonHiggsApi.DL.Entities
{
    public class User
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string NickName { get; set; }

        public string? IpAddress { get; set; }

        public string Token { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public ICollection<UserLevels>? UserLevels { get; set; }

        public ICollection<Message>? Messages { get; set; }
    }
}
