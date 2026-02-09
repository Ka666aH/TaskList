using Application.Interfaces.RepositoryInterfaces;
using Application.Interfaces.ServiceInterfaces;
using Domain.Entities;

namespace Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IUserRepository _ur;

        public ReportService(IUserRepository ur) => _ur = ur;

        public async Task<int> GetGoalsAmountAsync(CancellationToken ct = default) => await _ur.GetGoalsAmountAsync(ct);

        public async Task<int> GetUserGoalsAmountAsync(string login, CancellationToken ct = default) => await _ur.GetUserGoalsAmountAsync(login, ct);

        public async Task<int> GetUsersAmountAsync(CancellationToken ct = default) => await _ur.GetUsersAmountAsync(ct);

        public async Task<List<User>> GetUsersPageAsync(int pageSize, int page, CancellationToken ct = default) => await _ur.GetUsersPageAsync(pageSize, page, ct);
    }
}
