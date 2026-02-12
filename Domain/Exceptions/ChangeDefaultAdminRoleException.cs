namespace Domain.Exceptions
{
    public class ChangeDefaultAdminRoleException : AppException
    {
        private const string _code = "CANT_CHANGE_DEFAULT_ADMIN_ROLE";
        private const string _message = "The default admin role can't be changed.";
        public ChangeDefaultAdminRoleException() : base(_code, _message) { }
    }
}