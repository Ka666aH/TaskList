namespace Domain.Exceptions
{
    public class NewPasswordIsOld : AppException
    {
        private const string _code = "NEW_PASSWORD_IS_OLD";
        private const string _message = "The new password is the same as the old one.";
        public NewPasswordIsOld() : base(_code, _message) { }
    }
}