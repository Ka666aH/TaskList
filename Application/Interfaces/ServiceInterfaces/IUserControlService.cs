namespace Application.Interfaces.ServiceInterfaces
{
    public interface IUserControlService
    {
        Task<bool> ChangePasswordAsync(string login, string newPassword, CancellationToken ct = default);
        Task<bool> DeleteAccountAsync(string login, CancellationToken ct = default);
    }
}
