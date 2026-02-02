namespace Application.Interfaces.ServiceInterfaces
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(string login, string password, CancellationToken ct = default);
        Task<string> LoginAsync(string login, string password, CancellationToken ct = default);
    }
}
