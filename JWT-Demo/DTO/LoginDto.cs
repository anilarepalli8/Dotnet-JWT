using JWT_Demo.CustomValidations;

namespace JWT_Demo.DTO
{
    public class LoginDto
    {
        public string Username { get; set; }

        [Password]
        public string Password { get; set; }
    }
}
