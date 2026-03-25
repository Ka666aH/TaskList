using Domain.Constants;
using Presentation.DTO;
using System.Net.Http.Json;

namespace TaskList.Tests
{
    public class AdminControllerTestsBase : IntergrationTestsBase
    {
        protected async Task<HttpResponseMessage> ChangeRole(string login, RoleType role) =>
            await _httpClient.PatchAsJsonAsync($"admin/users/{login}/role", new ChangeRoleRequest(role.ToString()), TestContext.Current.CancellationToken);
    }
}