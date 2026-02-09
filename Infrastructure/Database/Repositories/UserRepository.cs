using Application.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly EFCDbContext _db;

        public UserRepository(EFCDbContext db)
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
            return await _db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == login, ct);
        }

        public async Task<User?> GetUserTrackAsync(string login, CancellationToken ct = default)
        {
            return await _db.Users.FindAsync(login, ct);
        }

        public async Task<User?> GetUserWithGoalsAsync(string login, CancellationToken ct = default)
        {
            return await _db.Users
                .AsNoTracking()
                .Include(u => u.Goals)
                .FirstOrDefaultAsync(u => u.Login == login, ct);
        }

        public async Task<User?> GetUserWithGoalsTrackAsync(string login, CancellationToken ct = default)
        {
            return await _db.Users
                .Include(u => u.Goals)
                .FirstOrDefaultAsync(u => u.Login == login, ct);
        }

        public async Task<int> GetUsersAmountAsync(CancellationToken ct = default)
        {
            return await _db.Users
                .AsNoTracking()
                .CountAsync(ct);
        }
        public async Task<List<User>> GetUsersAsync(CancellationToken ct = default)
        {
            return await _db.Users
                .AsNoTracking()
                .ToListAsync(ct);
        }
        public async Task<List<User>> GetUsersPageAsync(int pageSize, int page, CancellationToken ct = default)
        {
            if (page < 1) throw new ArgumentException("Page must be >= 1.");

            return await _db.Users
                .AsNoTracking()
                .OrderBy(u => u.Login)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);
        }
        public async Task<int> GetGoalsAmountAsync(CancellationToken ct = default)
        {
            return await _db.Goals
                .AsNoTracking()
                .CountAsync(ct);
        }

        public async Task<int> GetUserGoalsAmountAsync(string login, CancellationToken ct = default)
        {
            return await _db.Goals
                .AsNoTracking()
                .CountAsync(g => g.UserLogin == login, ct);
        }
    }
}