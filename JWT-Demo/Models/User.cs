using JWT_Demo.CustomValidations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JWT_Demo.Models
{
    public class User
    {
        [Key]
        [StringLength(10)]
        public string UserId { get; set; } // Primary key in the format "SBI12345"

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(256)]
        [Password]
        public string Password { get; set; } // Hashed password

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(10)]
        public int AccountNumber { get; set; } // Auto-generated 10-digit account number

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Balance { get; set; } = 0; // Default balance is 0
    }
}
