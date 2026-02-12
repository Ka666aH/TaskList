namespace Domain.Exceptions
{
    public class GoalEmptyTitleException : AppException
    {
        private const string _code = "GOAL_TITLE_IS_EMPTY";
        private const string _message = "Title can't be empty.";
        public GoalEmptyTitleException() : base(_code, _message) { }
    }
}