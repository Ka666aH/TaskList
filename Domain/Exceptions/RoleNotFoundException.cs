namespace Domain.Exceptions
{
    public class RoleNotFoundException : AppException
    {
        private const string _code = "ROLE_NOT_FOUND";
        private const string _message = "Role not found.";
        public RoleNotFoundException() : base(_code, _message) { }
    }
}