using Application.Interfaces.RepositoryInterfaces;
using Microsoft.Extensions.DependencyInjection;
using Presentation.DTO;
using System.Net.Http.Json;

namespace TaskList.Tests
{
    public class UserControllerTestsBase : IntergrationTestsBase
    {
        protected IPasswordEncrypterRepository _passwordEncrypter =>
            _scope.ServiceProvider.GetRequiredService<IPasswordEncrypterRepository>();

        protected bool VerifyPassword(string password, string hashedPassword) => _passwordEncrypter.Verify(password, hashedPassword);

        protected async Task<HttpResponseMessage> DeleteAccount() =>
            await _httpClient.DeleteAsync("/account", TestContext.Current.CancellationToken);
        protected async Task<HttpResponseMessage> ChangePassword(string newPassword) =>
            await _httpClient.PatchAsJsonAsync("/account/password", new ChangePasswordRequest(newPassword), TestContext.Current.CancellationToken);
    }
}