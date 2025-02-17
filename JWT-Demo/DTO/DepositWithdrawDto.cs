using JWT_Demo.CustomValidations;

namespace JWT_Demo.DTO
{
    public class DepositWithdrawDto
    {
        [PositiveAmount]
        public decimal Amount { get; set; }
    }
}
