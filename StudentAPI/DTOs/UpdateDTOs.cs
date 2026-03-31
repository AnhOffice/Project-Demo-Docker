using System.ComponentModel.DataAnnotations;

namespace StudentAPI.DTOs
{
    public class UpdateDTOs
    {
        [Required, MaxLength(30)]
        public string UserName { get; set; }
        [Required, MaxLength(100)]
        public string FullName { get; set; }
        [EmailAddress, MaxLength(100)]
        public string Email { get; set; }
        [Required, MaxLength(255)]
        public string Password { get; set; }
        [Required, MaxLength(50)]
        public string Role { get; set; }
        
    }
}
