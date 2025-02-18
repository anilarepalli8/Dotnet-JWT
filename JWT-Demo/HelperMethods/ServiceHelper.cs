using JWT_Demo.CustomException;
using JWT_Demo.Models;

namespace JWT_Demo.HelperMethods
{
    public class ServiceHelper
    {
        private readonly BankDBContext _dbContext;

        public ServiceHelper(BankDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public User GetUser(string userId)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.UserId.Equals(userId));
            if (user == null) throw new Error("User not found");
            return user;
        }

        // Fetch user by AccountNumber or throw an error if not found
        public User GetUserByAccountNumberOrThrow(int accountNumber)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.AccountNumber == accountNumber);
            if (user == null) throw new Error("Receiver account not found");
            return user;
        }

        // Validate user's balance for withdrawals or transfers
        public void ValidateBalance(User user, decimal amount, string transactionType)
        {
            if ((transactionType == "Withdrawl" || transactionType == "Transfer") && user.Balance < amount)
                throw new Error("Insufficient Balance");
        }

        // Add a transaction record to the database
        public void AddTransaction(BankDBContext dbContext, string userId, string type, decimal amount, int? receiverAccountNumber = null)
        {
            var transaction = new Transaction
            {
                UserId = userId,
                Type = type,
                Amount = amount,
                ReceiverAccountNumber = receiverAccountNumber
            };
            dbContext.Transactions.Add(transaction);
        }
    }
}
