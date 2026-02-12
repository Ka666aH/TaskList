namespace Domain.Exceptions
{
    public class AppException : Exception
    {
        public string Code { get; init; }
        public AppException(string code, string message) : base(message) => Code = code;
        public AppException(string code, string message, Exception? inner) : base(message, inner) => Code = code;
    }
}