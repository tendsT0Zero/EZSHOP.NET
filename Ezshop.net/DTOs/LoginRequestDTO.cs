using System.ComponentModel.DataAnnotations;

namespace EzShop.Api.DTOs
{
    public class LoginRequestDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
