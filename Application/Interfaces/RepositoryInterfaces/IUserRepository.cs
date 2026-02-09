using Domain.Entities;

namespace Application.Interfaces.RepositoryInterfaces
{
    public interface IUserRepository
    {
        //User
        Task AddUserAsync(User user, CancellationToken ct = default);
        Task DeleteUserAsync(string login, CancellationToken ct = default);
        Task<User?> GetUserAsync(string login, CancellationToken ct = default);
        Task<User?> GetUserTrackAsync(string login, CancellationToken ct = default);
        Task<User?> GetUserWithGoalsAsync(string login, CancellationToken ct = default);
        Task<User?> GetUserWithGoalsTrackAsync(string login, CancellationToken ct = default);

        Task<int> GetUsersAmountAsync(CancellationToken ct = default);
        Task<List<User>> GetUsersAsync(CancellationToken ct = default);
        Task<List<User>> GetUsersPageAsync(int pageSize, int page,CancellationToken ct = default);
        Task<int> GetGoalsAmountAsync(CancellationToken ct = default);
        Task<int> GetUserGoalsAmountAsync(string login, CancellationToken ct = default);

        //Task UpdateUserAsync(User user, CancellationToken ct = default);

        ////Goals
        //Task<Goal> AddGoal(User user, Goal goal, CancellationToken ct = default);
        //Task UpdateGoalAsync(User user, Goal goal, CancellationToken ct = default);
        //Task DeleteGoalAsync(User user, Guid goalId, CancellationToken ct = default);
    }
}