using Domain.Entities;
using Infrastructure.Database;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Presentation.DTO;
using System.Net.Http.Headers;
using System.Net.Http.Json;
namespace TaskList.Tests
{
    public class IntergrationTestsBase : WebApplicationFactory<Program>, IAsyncLifetime
    {
        protected readonly HttpClient _httpClient;
        protected IServiceScope _scope = null!;

        protected const string _setCookieHeader = "Set-Cookie";
        public IntergrationTestsBase()
        {
            _httpClient = CreateClient();
        }
        protected EFCDbContext _db =>
            _scope.ServiceProvider.GetRequiredService<EFCDbContext>();

        public async ValueTask InitializeAsync()
        {
            _scope = Services.CreateScope();
            await ClearDB();
        }
        private async Task ClearDB()
        {
            var db = _scope.ServiceProvider.GetRequiredService<EFCDbContext>();
            await db.Goals
                .ExecuteDeleteAsync(TestContext.Current.CancellationToken);
            await db.Users
                .Where(x => x.Login != "admin")
                .ExecuteDeleteAsync(TestContext.Current.CancellationToken);
        }
        protected async Task<int> CountUsersInDatabase() =>
            await _db.Users.CountAsync(TestContext.Current.CancellationToken);
        protected async Task<User?> FindUserByLogin(string login) =>
            await _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Login == login, TestContext.Current.CancellationToken);

        protected async Task<HttpResponseMessage> Register(string login = "user", string password = "password")
        {
            return await _httpClient.PostAsJsonAsync(
                "/auth/reg",
                new UserRequest(login, password),
                TestContext.Current.CancellationToken);
        }
        protected async Task<HttpResponseMessage> LogIn(string login, string password)
        {
            var loginResponse = await _httpClient.PostAsJsonAsync(
                "/auth/login",
                new UserRequest(login, password),
                TestContext.Current.CancellationToken);

            if (loginResponse.Headers.Contains(_setCookieHeader))
            {
                var token = GetTokenFromResponse(loginResponse);
                SetTokenToHTTPClient(token);
            }

            return loginResponse;
        }
        private string GetTokenFromResponse(HttpResponseMessage response) =>
    response.Headers
    .GetValues(_setCookieHeader)
    .First(c => c.StartsWith("token="))
    .Split(';')
    .First()
    .Substring("token=".Length);
        private void SetTokenToHTTPClient(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
        protected async Task RegisterAndLogInClient(string login = "user", string password = "password")
        {
            await Register(login, password);
            await LogIn(login, password);
        }
        protected async Task<HttpResponseMessage> LogOut()
        {
            return await _httpClient.PostAsync("/auth/logout", null, TestContext.Current.CancellationToken);
        }
    }
}