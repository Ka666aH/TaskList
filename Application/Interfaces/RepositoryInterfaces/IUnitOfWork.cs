namespace Application.Interfaces.RepositoryInterfaces
{
    public interface IUnitOfWork
    {
        Task<bool> SaveChangesAsync(CancellationToken ct = default);
    }
}
