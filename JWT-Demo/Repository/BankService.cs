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
        private readonly GenerateUserID _generateUserId;
        private readonly GenerateAccountNumbeR _generateAccountNumber;
        private readonly ServiceHelper _serviceHelper;

        public BankService(
            BankDBContext bankDBContext,
            IMapper mapper,
            GenerateUserID generateUserID,
            GenerateAccountNumbeR generateAccountNumber,
            ServiceHelper serviceHelper)
        {
            _dbContext = bankDBContext;
            _mapper = mapper;
            _generateUserId = generateUserID;
            _generateAccountNumber = generateAccountNumber;
            _serviceHelper = serviceHelper;
        }

        public bool Register(RegisterDto registerDto)
        {
            var user = _mapper.Map<User>(registerDto);
            user.UserId = _generateUserId.GenerateUserId();
            user.AccountNumber = _generateAccountNumber.GenerateAccountNumber();
            user.Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            _dbContext.Users.Add(user);
            return _dbContext.SaveChanges() > 0;
        }

        public User Login(string userName, string password)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Username.Equals(userName));
            return user != null && BCrypt.Net.BCrypt.Verify(password, user.Password) ? user : null;
        }

        public User GetDetails(string userId)
        {
            return _serviceHelper.GetUser(userId);
        }

        public decimal Deposit(string userId, decimal amount)
        {
            return PerformTransaction(userId, amount, "Deposit");
        }
        public decimal Withdraw(string userId, decimal amount)
        {
            return PerformTransaction(userId, amount, "Withdrawl");
        }
        public decimal Transfer(string userId, decimal amount, int receiverAccountNumber)
        {
            return PerformTransaction(userId, amount, "Transfer", receiverAccountNumber);
        }

        public IEnumerable<Transaction> GetHistory(string userId)
        {
            return _dbContext.Transactions.Where(t => t.UserId == userId).ToList();
        }

        private decimal PerformTransaction(string userId, decimal amount, string transactionType, int? receiverAccountNumber = null)
        {
            var user = _serviceHelper.GetUser(userId);
            _serviceHelper.ValidateBalance(user, amount, transactionType);

            if (transactionType == "Transfer")
            {
                var receiver = _serviceHelper.GetUserByAccountNumberOrThrow(receiverAccountNumber.Value);
                receiver.Balance += amount;
            }

            user.Balance = transactionType switch
            {
                "Deposit" => user.Balance + amount,
                "Withdrawl" => user.Balance - amount,
                "Transfer" => user.Balance - amount,
                _ => throw new Error("Invalid Transaction Type")
            };

            _serviceHelper.AddTransaction(_dbContext, userId, transactionType, amount, receiverAccountNumber);
            
            if (!(_dbContext.SaveChanges() > 0))
                throw new Error("Unable to complete the transaction");

            return user.Balance;
        }
    }
}
