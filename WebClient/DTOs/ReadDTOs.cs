using System.ComponentModel.DataAnnotations;

namespace WebClient.DTOs
{
    public class ReadDTOs
    {
        public int Id { get; set; }
        [Required, MaxLength(30)]
        public string UserName { get; set; }
        [Required, MaxLength(100)]
        public string FullName { get; set; }
        [Required, MaxLength(100)]
        public string Email { get; set; }
        [Required, MaxLength(255)]
        public string Password { get; set; }
        [Required, MaxLength(50)]
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
