namespace Domain.Exceptions
{
    public class IncorrectPasswordException : AppException
    {
        private const string _code = "INCORRECT_PASSWORD";
        private const string _message = "Incorrect password.";
        public IncorrectPasswordException() : base(_code, _message) { }
    }
}