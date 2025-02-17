using AutoMapper;
using JWT_Demo.CustomException;
using JWT_Demo.DTO;
using JWT_Demo.HelperMethods;
using JWT_Demo.Models;

namespace JWT_Demo.Repository
{
    public class BankService : IBankRepository
    {
        private readonly BankDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly GenerateUserID generateUserId;
        private readonly GenerateAccountNumbeR generateAccountNumber;
        public BankService(BankDBContext bankDBContext,IMapper mapper, GenerateUserID generateUserID,GenerateAccountNumbeR generateAccountNumbeR) 
        {
            _dbContext = bankDBContext;
            _mapper = mapper;
            generateUserId = generateUserID;
            generateAccountNumber = generateAccountNumbeR;
        }

        public bool register(RegisterDto registerDto)
        {
            string userId = generateUserId.GenerateUserId();
            int accountNumber = generateAccountNumber.GenerateAccountNumber();

            var user = _mapper.Map<User>(registerDto);
            user.UserId= userId;
            user.AccountNumber = accountNumber;
            _dbContext.Users.Add(user);
            return _dbContext.SaveChanges()>0 ? true:false;
        }
        public User login(string userName, string password)
        {
           User user = _dbContext.Users.FirstOrDefault(u => u.Username.Equals(userName) && u.Password.Equals(password));
            return (user != null) ? user : null;
        }

        public User getDetails(string userId)
        {
            User user = _dbContext.Users.FirstOrDefault(u=>u.UserId.Equals(userId));
            return (user != null) ? user : null;
        }

        public decimal deposit(string userId, decimal amount)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.UserId.Equals(userId));
            user.Balance += amount;
            var transaction = new Transaction()
            {
                UserId = userId,
                Type = "Deposit",
                Amount = amount
            };
            _dbContext.Transactions.Add(transaction);
            return (_dbContext.SaveChanges()>0) ? user.Balance : throw new Error("Unable to Deposit");
        }

        public decimal withdraw(string userId,decimal amount)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.UserId.Equals(userId));
              if (user.Balance < amount) throw new Error("Insufficient Balance");
            user.Balance -= amount;
            var transaction = new Transaction()
            {
                UserId = userId,
                Type = "Withdrawl",
                Amount = amount
            };
            _dbContext.Transactions.Add(transaction);
            return (_dbContext.SaveChanges() > 0) ? user.Balance : throw new Error("Unable to Withdraw");
        }

        public decimal transfer(string userId, decimal amount, int accountnumber)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.UserId.Equals(userId));
              if (user.Balance < amount) throw new Error("Insufficient Balance to Transfer");
            var receiverAccount = _dbContext.Users.FirstOrDefault(u => u.AccountNumber.Equals(accountnumber));
            receiverAccount.Balance += amount;
            user.Balance -= amount;
            var transaction = new Transaction()
            {
                UserId = userId,
                Type = "Transfer",
                ReceiverAccountNumber =accountnumber,
                Amount = amount
            };
            _dbContext.Transactions.Add(transaction);
            return (_dbContext.SaveChanges() > 0) ? user.Balance : throw new Error("Unable to Transfer");
        }

        public IEnumerable<Transaction> getHistory(string userId)
        {
            return _dbContext.Transactions
                     .Where(t => t.UserId == userId)
                     .ToList();
        }
    }
}
