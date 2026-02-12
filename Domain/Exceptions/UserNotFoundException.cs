namespace Domain.Exceptions
{
    public class UserNotFoundException : AppException
    {
        private const string _code = "USER_NOT_FOUND";
        private const string _message = "User not found.";
        public UserNotFoundException() : base(_code, _message) { }
    }
}