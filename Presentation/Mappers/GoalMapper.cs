using Domain.Entities;
using Presentation.DTO;

namespace Presentation.Mappers
{
    public static class GoalMapper
    {
        public static Goal ToGoal(string login, GoalRequest request) => new Goal(login, request.Title, request.Description, request.Deadline);
        public static List<Goal> ToGoalList(string login, IEnumerable<GoalRequest> requests) => requests.Select(r => ToGoal(login, r)).ToList();
        public static GoalResponse ToResponse(Goal goal) => new GoalResponse(goal.Title, goal.Description, goal.CreateAt, goal.Deadline);
        public static List<GoalResponse> ToResponseList(IEnumerable<Goal> goals) => goals.Select(ToResponse).ToList();
    }
}
