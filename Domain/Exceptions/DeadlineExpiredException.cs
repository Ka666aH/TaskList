namespace Domain.Exceptions
{
    public class DeadlineExpiredException : AppException
    {
        private const string _code = "DEADLINE_EXPIRED";
        private const string _message = "Deadline already fucked up!";
        public DeadlineExpiredException() : base(_code, _message) { }
    }
}