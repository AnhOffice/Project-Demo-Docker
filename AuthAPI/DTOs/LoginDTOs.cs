using System.ComponentModel.DataAnnotations;

namespace AuthAPI.DTOs
{
    public class LoginDTOs
    {
        [Required]
        [MaxLength(50)]
        public string UsernameOrEmail { get; set; }

        [Required]
        public string PasswordHash { get; set; }
    }
}
