using Domain.Entities;

namespace Application.Interfaces.ServiceInterfaces
{
    public interface IGoalControlService
    {
        Task<bool> AddGoalAsync(string login, Goal goal, CancellationToken ct = default);
        Task<bool> UpdateGoalAsync(string login, Goal goal, CancellationToken ct = default);
        Task<bool> RemoveGoalAsync(string login, Goal goal, CancellationToken ct = default);
    }
}
