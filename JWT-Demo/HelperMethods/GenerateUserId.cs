using JWT_Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace JWT_Demo.HelperMethods
{
    public class GenerateUserID
    {
        private readonly BankDBContext bankDBContext;
        public GenerateUserID(BankDBContext bankDBContext)
        {
            this.bankDBContext = bankDBContext;
        }
        public string GenerateUserId()
        {
            string userId;
            do
            {
                userId = "SBI" + new Random().Next(10000, 99999).ToString();
            }
            while (bankDBContext.Users.Any(u => u.UserId == userId));
            return userId;
        }
    }
}
