using System.ComponentModel.DataAnnotations;

namespace BosonHiggsApi.BL.Models
{
    public class RegisterModel
    {
        [Required]
        [MaxLength(32)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(32)]
        public string NickName { get; set; }
    }
}
