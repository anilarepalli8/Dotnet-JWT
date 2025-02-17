using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JWT_Demo.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; } // Auto-incremented primary key

        [Required]
        [StringLength(10)]
        public string UserId { get; set; } // Foreign key referencing Users.UserId

        [Required]
        [StringLength(20)]
        public string Type { get; set; } // Type of transaction: "Deposit", "Withdraw", or "Transfer"


        public int? ReceiverAccountNumber { get; set; } = null;


        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; } // Amount involved in the transaction

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow; // Timestamp of the transaction

        [ForeignKey("UserId")]
        public virtual User User { get; set; } // Navigation property for the related User
    }
}
