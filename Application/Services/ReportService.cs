using Application.Interfaces.RepositoryInterfaces;
using Application.Interfaces.ServiceInterfaces;
using Domain.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IUserRepository _ur;
        private readonly IMemoryCache _cache;
        private readonly ICacheKeyService _cacheKey;

        public ReportService(IUserRepository ur, IMemoryCache cache, ICacheKeyService cacheKey)
        {
            _ur = ur;
            _cache = cache;
            _cacheKey = cacheKey;
        }

        public async Task<int> GetGoalsAmountAsync(CancellationToken ct = default)
        {
            return await _cache.GetOrCreateAsync(_cacheKey.GetGoalsAmountKey(), async factory =>
            {
                var result = await _ur.GetGoalsAmountAsync(ct);
                factory.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                return result;
            });
        }
        public async Task<int> GetUserGoalsAmountAsync(string login, CancellationToken ct = default)
        {
            return await _cache.GetOrCreateAsync(_cacheKey.GetUserGoalsAmountKey(login), async factory =>
            {
                var result = await _ur.GetUserGoalsAmountAsync(login, ct);
                factory.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                return result;
            });
        }

        public async Task<int> GetUsersAmountAsync(CancellationToken ct = default)
        {
            return await _cache.GetOrCreateAsync(_cacheKey.GetUsersAmountKey(), async factory =>
            {
                var result = await _ur.GetUsersAmountAsync(ct);
                factory.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                return result;
            });
        }
            
        public async Task<List<User>> GetUsersPageAsync(int pageSize, int page, CancellationToken ct = default) => await _ur.GetUsersPageAsync(pageSize, page, ct);
    }
}
