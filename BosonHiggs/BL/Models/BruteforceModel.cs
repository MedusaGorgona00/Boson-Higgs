using System.ComponentModel.DataAnnotations;

namespace BosonHiggsApi.BL.Models
{
    public class BruteforceModel
    {
        [Required]
        [MaxLength(32)]
        [EmailAddress]
        public string Email { get; set; }


        [Required]
        [MaxLength(32)]
        public string Password { get; set; }

        [Required]
        [MaxLength(32)]
        public string UserToken { get; set; }
    }
}
