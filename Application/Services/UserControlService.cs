using Application.Interfaces.RepositoryInterfaces;
using Application.Interfaces.ServiceInterfaces;
using Domain.Constants;
using Domain.Exceptions;

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
            return await _uow.SaveChangesAsync(ct);
        }
    }
}