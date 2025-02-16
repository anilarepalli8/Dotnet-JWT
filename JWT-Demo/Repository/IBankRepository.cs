using JWT_Demo.DTO;
using JWT_Demo.Models;

namespace JWT_Demo.Repository
{
    public interface IBankRepository
    {
        bool register(RegisterDto registerDto);
        User login(string userName,string password);

        User getDetails(string userId);

        decimal deposit(string userId,decimal amount);
        decimal withdraw(string userId, decimal amount);
        decimal transfer(string userId, decimal amount, int accountnumber);
        
    }
}
