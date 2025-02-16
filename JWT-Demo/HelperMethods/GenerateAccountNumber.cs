using JWT_Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace JWT_Demo.HelperMethods
{
    public class GenerateAccountNumbeR
    {
        private readonly BankDBContext bankDBContext;
        public GenerateAccountNumbeR(BankDBContext bankDBContext)
        {
            this.bankDBContext = bankDBContext;
        }
        public int GenerateAccountNumber()
        {
            int accountNumber;
            do
            {
                accountNumber = int.Parse(new Random().Next(1000000000, 2000000000).ToString());
            }
            while (bankDBContext.Users.Any(u => u.AccountNumber.Equals(accountNumber)));
            return accountNumber;
        }
    }
}
