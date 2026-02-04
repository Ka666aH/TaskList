using Application.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly PostgreSQLDbContext _db;

        public UserRepository(PostgreSQLDbContext db)
        {
            _db = db;
        }

        public async Task AddUserAsync(User user, CancellationToken ct = default)
        {
            var existingUser = await GetUserTrackAsync(user.Login, ct);
            if (existingUser != null) throw new ArgumentException("User with this login is already exist.");

            await _db.Users.AddAsync(user, ct);
        }

        public async Task DeleteUserAsync(string login, CancellationToken ct = default)
        {
            var existingUser = await GetUserTrackAsync(login, ct);
            if (existingUser == null) throw new NullReferenceException("User not found.");

            //await _db.Users.Where(u => u.Login == login).ExecuteDeleteAsync(ct);
            _db.Users.Remove(existingUser);
        }

        public async Task<User?> GetUserAsync(string login, CancellationToken ct = default)
        {
            return await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Login == login, ct);
        }
        public async Task<User?> GetUserTrackAsync(string login, CancellationToken ct = default)
        {
            return await _db.Users.FindAsync(login, ct);
        }

        public async Task<User?> GetUserWithGoalsAsync(string login, CancellationToken ct = default)
        {
            return await _db.Users.Include(u => u.Goals).AsNoTracking().FirstOrDefaultAsync(u => u.Login == login, ct);
        }

        public async Task<User?> GetUserWithGoalsTrackAsync(string login, CancellationToken ct = default)
        {
            return await _db.Users.Include(u => u.Goals).FirstOrDefaultAsync(u => u.Login == login, ct);
        }
    }
}