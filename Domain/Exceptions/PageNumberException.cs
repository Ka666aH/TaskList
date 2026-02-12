namespace Domain.Exceptions
{
    public class PageNumberException : AppException
    {
        private const string _code = "PAGE_NUMBER";
        private const string _message = "Page number must be >= 1.";
        public PageNumberException() : base(_code, _message) { }
    }
}