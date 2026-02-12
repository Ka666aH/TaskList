namespace Domain.Exceptions
{
    public class EnvException : AppException
    {
        public EnvException(string code, string message) : base(code, message) { }
        public EnvException(string code, string message, Exception? inner) : base(code, message, inner) { }
    }
}