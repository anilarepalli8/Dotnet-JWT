namespace JWT_Demo.DTO
{
    public class UserDetailsDto
    {
        public string UserId { get; set; } // Auto-generated "SBI" + unique number
        public string Username { get; set; }
        public string AccountNumber { get; set; } // Auto-generated 10-digit account number
        public decimal Balance { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
