namespace Domain.Exceptions
{
    public class DeleteDefaultAdminException : AppException
    {
        private const string _code = "CANT_DELETE_DEFAULT_ADMIN";
        private const string _message = "The default admin can't be deleted.";
        public DeleteDefaultAdminException() : base(_code, _message) { }
    }
}