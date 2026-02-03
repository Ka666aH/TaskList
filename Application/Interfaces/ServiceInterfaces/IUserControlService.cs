namespace Application.Interfaces.ServiceInterfaces
{
    public interface IUserControlService
    {
        Task<bool> ChangeHashedPasswordAsync(string login, string newHashedPass, CancellationToken ct = default);
        Task<bool> DeleteAccountAsync(string login, CancellationToken ct = default);
    }
}
