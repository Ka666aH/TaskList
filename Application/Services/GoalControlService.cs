using Application.Interfaces.RepositoryInterfaces;
using Application.Interfaces.ServiceInterfaces;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Services
{
    public class GoalControlService : IGoalControlService
    {
        private readonly IUserRepository _ur;
        private readonly IUnitOfWork _uow;
        private readonly IMemoryCache _cache;
        private readonly ICacheKeyService _cacheKey;

        public GoalControlService(IUserRepository ur, IUnitOfWork uow, IMemoryCache cache, ICacheKeyService cacheKey)
        {
            _ur = ur;
            _uow = uow;
            _cache = cache;
            _cacheKey = cacheKey;
        }

        public async Task<bool> AddGoalAsync(string login, Goal goal, CancellationToken ct = default)
        {
            var existingUser = await _ur.GetUserTrackAsync(login, ct);
            if (existingUser == null) throw new UserNotFoundException();

            existingUser.AddGoal(goal);
            var result = await _uow.SaveChangesAsync(ct);
            if (result)
            {
                _cache.Remove(_cacheKey.GetUserWithGoalsKey(login));
                _cache.Remove(_cacheKey.GetUserGoalsAmountKey(login));
                _cache.Remove(_cacheKey.GetGoalsAmountKey());
            }
            return result;
        }

        public async Task<User> GetUserAsync(string login, CancellationToken ct = default)
        {
            var userWithGoals = await _cache.GetOrCreateAsync(_cacheKey.GetUserWithGoalsKey(login), async factory =>
            {
                var existingUserWithGoals = await _ur.GetUserWithGoalsAsync(login, ct);

                factory.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

                return existingUserWithGoals;
            });
            return userWithGoals ?? throw new UserNotFoundException();
        }

        public async Task<User> GetUserTrackAsync(string login, CancellationToken ct = default)
        {

            var existingUserWithGoals = await _ur.GetUserWithGoalsTrackAsync(login, ct);
            if (existingUserWithGoals == null) throw new UserNotFoundException();

            return existingUserWithGoals;
        }

        public async Task<bool> RemoveGoalAsync(string login, Goal goal, CancellationToken ct = default)
        {
            var existingUser = await _ur.GetUserWithGoalsTrackAsync(login, ct);
            if (existingUser == null) throw new UserNotFoundException();

            existingUser.RemoveGoal(goal);
            var result = await _uow.SaveChangesAsync(ct);
            if (result)
            {
                _cache.Remove(_cacheKey.GetUserWithGoalsKey(login));
                _cache.Remove(_cacheKey.GetUserGoalsAmountKey(login));
                _cache.Remove(_cacheKey.GetGoalsAmountKey());
            }
            return result;
        }

        public async Task<bool> UpdateGoalAsync(string login, Guid goalId, Goal goal, CancellationToken ct = default)
        {
            var existingUser = await _ur.GetUserWithGoalsTrackAsync(login, ct);
            if (existingUser == null) throw new UserNotFoundException();

            var oldGoal = existingUser.Goals.FirstOrDefault(g => g.Id == goalId);
            if (oldGoal == null) throw new GoalNotFoundException();

            oldGoal.SetTitle(goal.Title);
            oldGoal.SetDescription(goal.Description);
            oldGoal.SetDeadline(goal.Deadline);

            var result = await _uow.SaveChangesAsync(ct);
            if (result) _cache.Remove(_cacheKey.GetUserWithGoalsKey(login));
            return result;
        }
    }
}