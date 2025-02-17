using System.ComponentModel.DataAnnotations;

namespace JWT_Demo.CustomValidations
{
    public class PositiveAmountAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is decimal amount)
            {
                if (amount <= 0)
                    return new ValidationResult("Amount must be greater than 0.");
            }
            return ValidationResult.Success;
        }
    }
}
