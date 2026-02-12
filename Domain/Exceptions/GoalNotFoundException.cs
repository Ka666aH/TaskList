namespace Domain.Exceptions
{
    public class GoalNotFoundException : AppException
    {
        private const string _code = "GOAL_NOT_FOUND";
        private const string _message = "Goal not found.";
        public GoalNotFoundException() : base(_code, _message) { }
    }
}