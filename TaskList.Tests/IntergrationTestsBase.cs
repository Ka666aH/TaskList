using Infrastructure.Database;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Presentation.DTO;
using System.Net.Http.Json;
namespace TaskList.Tests
{
    public class IntergrationTestsBase : WebApplicationFactory<Program>, IAsyncLifetime
    {
        protected readonly HttpClient _httpClient;
        protected IServiceScope _scope = null!;

        public IntergrationTestsBase()
        {
            _httpClient = CreateClient();
        }

        public async ValueTask InitializeAsync()
        {
            _scope = Services.CreateScope();
            await ClearDBAsync();
        }
        private async Task ClearDBAsync()
        {
            var db = _scope.ServiceProvider.GetRequiredService<EFCDbContext>();
            await db.Goals
                .ExecuteDeleteAsync(TestContext.Current.CancellationToken);
            await db.Users
                .Where(x=> x.Login != "admin")
                .ExecuteDeleteAsync(TestContext.Current.CancellationToken);
        }
        protected async Task<HttpResponseMessage> RegisterUser(string login, string password)
        {
            return await _httpClient.PostAsJsonAsync(
                "/auth/reg",
                new UserRequest(login, password), 
                TestContext.Current.CancellationToken);
        }
        protected async Task<HttpResponseMessage> LogInUser(string login, string password)
        {
            return await _httpClient.PostAsJsonAsync(
                "/auth/login",
                new UserRequest(login, password),
                TestContext.Current.CancellationToken);
        }
    }
}