using Application.Interfaces.RepositoryInterfaces;
using Application.Interfaces.ServiceInterfaces;
using Domain.Constants;
using Domain.Exceptions;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Services
{
    public class UserControlService : IUserControlService
    {
        private readonly IUserRepository _ur;
        private readonly IUnitOfWork _uow;
        private readonly IPasswordEncrypterRepository _per;

        private readonly IMemoryCache _cache;
        private readonly ICacheKeyService _cacheKey;


        public UserControlService(IUserRepository ur, IUnitOfWork uow, IPasswordEncrypterRepository per, IMemoryCache cache, ICacheKeyService cacheKey)
        {
            _ur = ur;
            _uow = uow;
            _per = per;
            _cache = cache;
            _cacheKey = cacheKey;
        }

        public async Task<bool> ChangePasswordAsync(string login, string newPassword, CancellationToken ct = default)
        {
            var existingUser = await _ur.GetUserTrackAsync(login, ct) ?? throw new UserNotFoundException();
            var newHashedPassword = _per.Encrypt(newPassword);
            existingUser.SetHashedPassword(newHashedPassword);
            return await _uow.SaveChangesAsync(ct);
        }

        public async Task<bool> ChangeRoleAsync(string login, RoleType newRole, CancellationToken ct = default)
        {
            if (login == DefaultAdmin.Login) throw new ChangeDefaultAdminRoleException();

            var existingUser = await _ur.GetUserTrackAsync(login, ct) ?? throw new UserNotFoundException();
            existingUser.SetUserRoleId((int)newRole);
            return await _uow.SaveChangesAsync(ct);
        }

        public async Task<bool> DeleteAccountAsync(string login, CancellationToken ct = default)
        {
            if (login == DefaultAdmin.Login) throw new DeleteDefaultAdminException();

            await _ur.DeleteUserAsync(login, ct);
            var result = await _uow.SaveChangesAsync(ct);
            if (result)
            {
                _cache.Remove(_cacheKey.GetUserGoalsAmountKey(login));
                _cache.Remove(_cacheKey.GetUserWithGoalsKey(login));
                _cache.Remove(_cacheKey.GetUsersAmountKey());
            }
            return result;
        }
    }
}