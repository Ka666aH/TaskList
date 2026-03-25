using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Presentation.DTO;
using System.Net.Http.Json;

namespace TaskList.Tests
{
    public class GoalControllerTestsBase : IntergrationTestsBase
    {
        protected async Task<HttpResponseMessage> AddGoal(string title, string? description, DateTime? deadline) =>
            await _httpClient.PostAsJsonAsync("/goals", new GoalRequest(title, description, deadline), TestContext.Current.CancellationToken);
        protected async Task<HttpResponseMessage> GetGoals() =>
            await _httpClient.GetAsync("/goals", TestContext.Current.CancellationToken);
        protected async Task<HttpResponseMessage> GetGoal(string goalId) =>
            await _httpClient.GetAsync($"/goals/{goalId}", TestContext.Current.CancellationToken);
        protected async Task<HttpResponseMessage> DeleteGoal(string goalId) =>
            await _httpClient.DeleteAsync($"/goals/{goalId}", TestContext.Current.CancellationToken);
        protected async Task<HttpResponseMessage> UpdateGoal(string goalId, GoalRequest goalRequest) =>
            await _httpClient.PutAsJsonAsync($"/goals/{goalId}", goalRequest, TestContext.Current.CancellationToken);

        protected async Task<IReadOnlyList<Goal>> FindUserGoals(string login)
        {
            var user = await _db.Users
                .AsNoTracking()
                .Include(u => u.Goals)
                .FirstAsync(u => u.Login == login, TestContext.Current.CancellationToken);
            return user.Goals;
        }
    }
}