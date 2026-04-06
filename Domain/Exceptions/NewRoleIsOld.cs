namespace Domain.Exceptions
{
    public class NewRoleIsOld : AppException
    {
        private const string _code = "NEW_ROLE_IS_OLD";
        private const string _message = "The new role is the same as the old one.";
        public NewRoleIsOld() : base(_code, _message) { }
    }
}