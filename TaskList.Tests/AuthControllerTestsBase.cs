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
        protected EFCDbContext _db =>
            _scope.ServiceProvider.GetRequiredService<EFCDbContext>();
        protected IPasswordEncrypterRepository _passwordEncrypter =>
            _scope.ServiceProvider.GetRequiredService<IPasswordEncrypterRepository>();
        protected async Task<int> CountUsersInDatabase() =>
            await _db.Users.CountAsync(TestContext.Current.CancellationToken);
        protected async Task<User?> FindUserByLogin(string login) =>
            await _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Login == login, TestContext.Current.CancellationToken);
        protected string GetTokenFromResponse(HttpResponseMessage response) =>
            response.Headers
            .GetValues("Set-Cookie")
            .First(c => c.StartsWith("token="))
            .Split(';')
            .First()
            .Substring("token=".Length);
        protected void SetTokenToHTTPClient(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
        protected bool VerifyPassword(string password, string hashedPassword) => _passwordEncrypter.Verify(password, hashedPassword);
    }
}
