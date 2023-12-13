using System.ComponentModel.DataAnnotations;

namespace BosonHiggsApi.BL.Models
{
    public class RegisterModel
    {
        [Required]
        [MaxLength(64)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(64)]
        public string NickName { get; set; }
    }
}
