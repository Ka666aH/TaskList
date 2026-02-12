namespace Domain.Exceptions
{
    public class UserEmptyLoginException : AppException
    {
        private const string _code = "USER_LOGIN_IS_EMPTY";
        private const string _message = "Login can't be empty.";
        public UserEmptyLoginException() : base(_code, _message) { }
    }
}