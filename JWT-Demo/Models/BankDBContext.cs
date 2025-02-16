using Microsoft.EntityFrameworkCore;

namespace JWT_Demo.Models
{
    public class BankDBContext:DbContext
    {
        public BankDBContext(DbContextOptions<BankDBContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
