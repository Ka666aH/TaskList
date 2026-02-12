namespace Domain.Exceptions
{
    public class LoginExistException : AppException
    {
        private const string _code = "LOGIN_IS_EXIST";
        private const string _message = "User with this login is already exist.";
        public LoginExistException() : base(_code, _message) { }
    }
}