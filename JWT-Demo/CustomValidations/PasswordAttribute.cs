using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace JWT_Demo.CustomValidations
{
    public class PasswordAttribute:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult("Password is required.");
            }

            string password = value.ToString();

            // Rule 1: At least 8 characters
            if (password.Length < 8)
            {
                return new ValidationResult("Password must be at least 8 characters long.");
            }

            // Rule 2: At least one number
            if (!Regex.IsMatch(password, @"\d"))
            {
                return new ValidationResult("Password must contain at least one number.");
            }

            // Rule 3: At least one special character
            if (!Regex.IsMatch(password, @"[!@#$%^&*(),.?""{}|<>]"))
            {
                return new ValidationResult("Password must contain at least one special character.");
            }

            // If all rules are satisfied
            return ValidationResult.Success;
        }
    }
}
