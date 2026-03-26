using Domain.Constants;
using Presentation.DTO;
using System.Net.Http.Json;

namespace TaskList.Tests
{
    public class AdminControllerTestsBase : IntergrationTestsBase
    {
        protected async Task<HttpResponseMessage> AddGoal(string title = "title", string? description = "description", DateTime? deadline = null) =>
            await _httpClient.PostAsJsonAsync("/goals", new GoalRequest(title, description, deadline), TestContext.Current.CancellationToken);
        protected async Task<HttpResponseMessage> ChangeRole(string login, RoleType role) =>
            await _httpClient.PatchAsJsonAsync($"admin/users/{login}/role", new ChangeRoleRequest(role.ToString()), TestContext.Current.CancellationToken);
        protected async Task<HttpResponseMessage> GetUsersAmount() =>
            await _httpClient.GetAsync($"admin/users/amount/", TestContext.Current.CancellationToken);
        protected async Task<HttpResponseMessage> GetGoalsAmount() =>
            await _httpClient.GetAsync($"admin/goals/amount/", TestContext.Current.CancellationToken);
        protected async Task<HttpResponseMessage> GetUsersPage(int pageSize, int page) =>
            await _httpClient.GetAsync($"/admin/users?pageSize={pageSize}&page={page}", TestContext.Current.CancellationToken);
        protected async Task<HttpResponseMessage> GetUserGoalsAmount(string login) =>
            await _httpClient.GetAsync($"/goals/amount/{login}", TestContext.Current.CancellationToken);

    }
}