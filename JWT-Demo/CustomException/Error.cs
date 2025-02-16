namespace JWT_Demo.CustomException
{
    public class Error : Exception
    {
        public Error(string? message) : base(message) { }
    }
}
