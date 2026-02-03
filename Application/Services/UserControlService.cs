using Application.Interfaces.RepositoryInterfaces;
using Application.Interfaces.ServiceInterfaces;

namespace Application.Services
{
    public class UserControlService : IUserControlService
    {
        private readonly IUserRepository _ur;
        private readonly IUnitOfWork _uow;

        public UserControlService(IUserRepository ur, IUnitOfWork uow)
        {
            _ur = ur;
            _uow = uow;
        }

        public async Task<bool> ChangeHashedPasswordAsync(string login, string newHashedPassword, CancellationToken ct = default)
        {
            var existingUser = await _ur.GetUserWithTrackingAsync(login, ct);
            if (existingUser == null) throw new NullReferenceException("User not found.");

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