namespace Domain.Exceptions
{
    public class UserEmptyHashedPasswordException : AppException
    {
        private const string _code = "USER_HASHED_PASSWORD_IS_EMPTY";
        private const string _message = "Hashed password can't be empty.";
        public UserEmptyHashedPasswordException() : base(_code, _message) { }
    }
}