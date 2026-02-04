using Application.Interfaces.RepositoryInterfaces;
using Application.Interfaces.ServiceInterfaces;

namespace Application.Services
{
    public class UserControlService : IUserControlService
    {
        private readonly IUserRepository _ur;
        private readonly IUnitOfWork _uow;
        private readonly IPasswordEncrypterRepository _per;

        public UserControlService(IUserRepository ur, IUnitOfWork uow, IPasswordEncrypterRepository per)
        {
            _ur = ur;
            _uow = uow;
            _per = per;
        }

        public async Task<bool> ChangePasswordAsync(string login, string newPassword, CancellationToken ct = default)
        {
            var existingUser = await _ur.GetUserTrackAsync(login, ct);
            if (existingUser == null) throw new NullReferenceException("User not found.");

            var newHashedPassword = _per.Encrypt(newPassword);
            existingUser.SetHashedPassword(newHashedPassword);
            return await _uow.SaveChangesAsync(ct);
        }

        public async Task<bool> DeleteAccountAsync(string login, CancellationToken ct = default)
        {
            await _ur.DeleteUserAsync(login, ct);
            return await _uow.SaveChangesAsync(ct);
        }
    }
}