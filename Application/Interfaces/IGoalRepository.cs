using Domain.Entities;

namespace Application.Interfaces
{
    public interface IGoalRepository
    {
        Task AddGoalAsync(string login, Goal goal, CancellationToken ct = default);
        Task<Goal> GetGoalByIdAsync(Guid goalId, CancellationToken ct = default);
        Task UpdateGoalAsync(Guid goalId, string newTitle, string? newDescription, DateTime? newDeadline, CancellationToken ct = default);
        Task RemoveGoalAsync(string login, Goal goal, CancellationToken ct = default);
    }
}