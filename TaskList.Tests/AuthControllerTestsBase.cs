using Application.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

namespace TaskList.Tests
{
    public class AuthControllerTestsBase : IntergrationTestsBase
    {
        protected IPasswordEncrypterRepository _passwordEncrypter =>
            _scope.ServiceProvider.GetRequiredService<IPasswordEncrypterRepository>();

        protected bool VerifyPassword(string password, string hashedPassword) => _passwordEncrypter.Verify(password, hashedPassword);

        protected async Task<HttpResponseMessage> LogOut()
        {
            return await _httpClient.PostAsync("/auth/logout", null, TestContext.Current.CancellationToken);
        }
    }
}
