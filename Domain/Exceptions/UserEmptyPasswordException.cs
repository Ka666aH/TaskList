namespace Domain.Exceptions
{
    public class UserEmptyPasswordException : AppException
    {
        private const string _code = "USER__PASSWORD_IS_EMPTY";
        private const string _message = "Password can't be empty.";
        public UserEmptyPasswordException() : base(_code, _message) { }
    }
}