using Application.Interfaces.RepositoryInterfaces;

namespace Infrastructure.Database.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EFCDbContext _db;

        public UnitOfWork(EFCDbContext db)
        {
            _db = db;
        }

        public async Task<bool> SaveChangesAsync(CancellationToken ct = default)
        {
            var changes = await _db.SaveChangesAsync(ct);
            return changes > 0;
        }
    }
}
