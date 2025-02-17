using JWT_Demo.CustomValidations;

namespace JWT_Demo.DTO
{
    public class TransferDto
    {
        public int ReceiverAccountNumber { get; set; }

        [PositiveAmount]
        public decimal Amount { get; set; }
    }
}
