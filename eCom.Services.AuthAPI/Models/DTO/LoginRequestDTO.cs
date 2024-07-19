using System.ComponentModel.DataAnnotations;

namespace eCom.Services.AuthAPI.Models.DTO
{
    public class LoginRequestDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
