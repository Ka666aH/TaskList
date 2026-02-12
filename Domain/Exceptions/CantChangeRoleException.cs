namespace Domain.Exceptions
{
    public class CantChangeRoleException : AppException
    {
        private const string _code = "CANT_CHANGE_ROLE";
        private const string _message = "Can't change role.";
        public CantChangeRoleException() : base(_code, _message) { }
    }
}