using Application.Interfaces.RepositoryInterfaces;
using Application.Interfaces.ServiceInterfaces;
using Domain.Entities;

namespace Application.Services
{
    public class GoalControlService : IGoalControlService
    {
        private readonly IUserRepository _ur;
        private readonly IUnitOfWork _uow;

        public GoalControlService(IUserRepository ur, IUnitOfWork uow)
        {
            _ur = ur;
            _uow = uow;
        }

        public async Task<bool> AddGoalAsync(string login, Goal goal, CancellationToken ct = default)
        {
            var existingUser = await _ur.GetUserTrackAsync(login, ct);
            if (existingUser == null) throw new NullReferenceException("User not found.");

            existingUser.AddGoal(goal);
            return await _uow.SaveChangesAsync(ct);
        }

        public async Task<User> GetUserAsync(string login, CancellationToken ct = default)
        {
            var existingUserWithGoals = await _ur.GetUserWithGoalsAsync(login, ct);
            if (existingUserWithGoals == null) throw new NullReferenceException("User not found.");

            return existingUserWithGoals;
        }

        public async Task<User> GetUserTrackAsync(string login, CancellationToken ct = default)
        {
            var existingUserWithGoals = await _ur.GetUserWithGoalsTrackAsync(login, ct);
            if (existingUserWithGoals == null) throw new NullReferenceException("User not found.");

            return existingUserWithGoals;
        }

        public async Task<bool> RemoveGoalAsync(string login, Goal goal, CancellationToken ct = default)
        {
            var existingUser = await _ur.GetUserWithGoalsTrackAsync(login, ct);
            if (existingUser == null) throw new NullReferenceException("User not found.");

            existingUser.RemoveGoal(goal);
            return await _uow.SaveChangesAsync(ct);
        }

        public async Task<bool> UpdateGoalAsync(string login, Guid goalId, Goal goal, CancellationToken ct = default)
        {
            var existingUser = await _ur.GetUserWithGoalsTrackAsync(login, ct);
            if (existingUser == null) throw new NullReferenceException("User not found.");

            var oldGoal = existingUser.Goals.FirstOrDefault(g => g.Id == goalId);
            if (oldGoal == null) throw new NullReferenceException("Goal not found.");

            oldGoal.SetTitle(goal.Title);
            oldGoal.SetDescription(goal.Description);
            oldGoal.SetDeadline(goal.Deadline);

            return await _uow.SaveChangesAsync(ct);
        }
    }
}