using JWT_Demo.DTO;
using JWT_Demo.Models;
using System.Collections.Generic;

namespace JWT_Demo.Repository
{
    public interface IBankRepository
    {
        bool Register(RegisterDto registerDto);
        User Login(string userName,string password);
        User GetDetails(string userId);
        decimal Deposit(string userId,decimal amount);
        decimal Withdraw(string userId, decimal amount);
        decimal Transfer(string userId, decimal amount, int accountnumber);
        IEnumerable<Transaction> GetHistory(string userId);

        
    }
}
