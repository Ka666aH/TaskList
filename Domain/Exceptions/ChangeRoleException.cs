namespace Domain.Exceptions
{
    public class ChangeRoleException : AppException
    {
        private const string _code = "CANT_CHANGE_ROLE";
        private const string _message = "Can't change role.";
        public ChangeRoleException() : base(_code, _message) { }
    }
}