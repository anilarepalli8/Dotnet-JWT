using JWT_Demo.CustomValidations;
using System.ComponentModel.DataAnnotations;

namespace JWT_Demo.DTO
{
    public class RegisterDto
    {
        public string Username { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        [Password]
        public string Password { get; set; }
    }
}
