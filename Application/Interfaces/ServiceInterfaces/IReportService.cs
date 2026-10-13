using Application.DTO;
using Domain.Entities;

namespace Application.Interfaces.ServiceInterfaces
{
    public interface IReportService
    {
        Task<int> GetUsersAmountAsync(CancellationToken ct = default);
        Task<PageResult<User>> GetUsersPageAsync(int pageSize, int page, CancellationToken ct = default);
        Task<int> GetGoalsAmountAsync(CancellationToken ct = default);
        Task<int> GetUserGoalsAmountAsync(string login, CancellationToken ct = default);
    }
}
