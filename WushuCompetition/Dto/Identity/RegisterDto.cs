using System.ComponentModel.DataAnnotations;

namespace WushuCompetition.Dto.Identity
{
    public class RegisterDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 4)]
        public string Password { get; set; }
    }
}
