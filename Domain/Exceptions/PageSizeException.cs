namespace Domain.Exceptions
{
    public class PageSizeException : AppException
    {
        private const string _code = "PAGE_SIZE";
        private const string _message = "Page size must be >= 1.";
        public PageSizeException() : base(_code, _message) { }
    }
}