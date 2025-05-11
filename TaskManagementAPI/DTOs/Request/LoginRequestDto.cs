using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.DTOs.Request
{
    public class LoginRequestDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
