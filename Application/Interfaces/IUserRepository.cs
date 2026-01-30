using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User> AddUserAsync(string login, string hashedPassword, CancellationToken ct = default);
        Task<User> GetUserByLoginAsync(string login, CancellationToken ct = default);
        Task UpdateUserHashedPasswordAsync(string login, string hashedPassword, CancellationToken ct = default);
        Task DeleteUserAsync(string login, CancellationToken ct = default);

    }
}