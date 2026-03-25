using Domain.Entities;
using FluentAssertions;
using FluentAssertions.Extensions;
using Presentation.DTO;
using System.Net;
using System.Net.Http.Json;

namespace TaskList.Tests
{
    [Collection("Integration")]
    public class GoalControllerTests : GoalControllerTestsBase
    {
        [Theory]
        [MemberData(nameof(GetAddGoalValidData), MemberType = typeof(GoalControllerTests))]
        public async Task Add_goal_pass(string? description, int? hoursToDeadline)
        {
            //Arrange
            string login = "user";
            await RegisterAndLogInClient(login: login);

            var deadline = hoursToDeadline != null
                ? DateTime.UtcNow.AddHours((double)hoursToDeadline)
                : (DateTime?)null;
            //Act
            var addGoalResopnse = await AddGoal("title", description, deadline);
            //Assert
            addGoalResopnse.StatusCode.Should().Be(HttpStatusCode.Created);

            var goals = await FindUserGoals(login);
            goals.Count.Should().Be(1);
            var goal = goals[0];
            goal.Title.Should().Be("title");
            goal.Description.Should().Be(description);
            if (deadline.HasValue) goal.Deadline.Should().BeCloseTo(deadline.Value, 1.Seconds());
            else goal.Deadline.Should().BeNull();
            goal.UserLogin.Should().Be(login);
            goal.CreateAt.Should().BeCloseTo(DateTime.UtcNow, 5.Seconds());

        }
        public static TheoryData<string?, int?> GetAddGoalValidData()
        {
            return new TheoryData<string?, int?>
            {
                { "description", 7*24 },
                { "", 1 },
                //{ "", null },
                { null, null }
            };
        }
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        public async Task Add_goal_fails_when_title_is_empty(string title)
        {
            //Arrange
            string login = "user";
            await RegisterAndLogInClient(login: login);
            //Act
            var addGoalResopnse = await AddGoal(title, "description", DateTime.UtcNow.AddDays(7));
            //Assert
            addGoalResopnse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var goals = await FindUserGoals(login);
            goals.Count.Should().Be(0);
        }
        [Fact]
        public async Task Add_goal_fails_when_deadline_expired()
        {
            //Arrange
            string login = "user";
            await RegisterAndLogInClient(login: login);
            //Act
            var addGoalResopnse = await AddGoal("title", "description", DateTime.UtcNow.AddDays(-1));
            //Assert
            addGoalResopnse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var goals = await FindUserGoals(login);
            goals.Count.Should().Be(0);
        }
        [Fact]
        public async Task Get_user_goals_pass()
        {
            //Assert
            string login = "user";
            await RegisterAndLogInClient(login: login);
            await AddGoal("title", "description", DateTime.UtcNow.AddDays(7));
            await AddGoal("title2", "description2", DateTime.UtcNow.AddDays(7));
            //Act
            var getGoalsResponse = await GetGoals();
            //Arrange
            getGoalsResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseGoals = await getGoalsResponse.Content.ReadFromJsonAsync<List<GoalResponse>>(TestContext.Current.CancellationToken);
            responseGoals.Should().NotBeNull();
            responseGoals.Count.Should().Be(2);
            responseGoals.Should().Contain(g => g.Title == "title" && g.Description == "description")
                .And.Contain(g => g.Title == "title2" && g.Description == "description2");

            var dbGoals = await FindUserGoals(login);
            dbGoals.Should().NotBeNull();
            dbGoals.Count.Should().Be(2);
            dbGoals.Should()
                .Contain(g => g.Title == "title")
                .And.Contain(g => g.Title == "title2");
        }
        [Fact]
        public async Task Get_user_goal_pass()
        {
            //Arrange
            string login = "user";
            await RegisterAndLogInClient(login: login);
            var addGoalResponse = await AddGoal("title", "description", DateTime.UtcNow.AddDays(7));
            var goalId = addGoalResponse.Headers.Location!.ToString().Split('/').Last();
            //Act
            var getGoalResponse = await GetGoal(goalId);
            //Assert
            getGoalResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseGoal = await getGoalResponse.Content.ReadFromJsonAsync<GoalResponse>(TestContext.Current.CancellationToken);
            responseGoal.Should().NotBeNull();
            responseGoal.Id.ToString().Should().Be(goalId);
            //responseGoal.Title.Should().Be("title");
            //responseGoal.Description.Should().Be("description");
        }
        [Fact]
        public async Task Get_user_goal_fails_when_goal_is_not_exist()
        {
            //Arrange
            string login = "user";
            await RegisterAndLogInClient(login: login);
            //Act
            var getGoalResponse = await GetGoal(Guid.NewGuid().ToString());
            //Assert
            getGoalResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task Delete_goal_pass()
        {
            //Arrange
            string login = "user";
            await RegisterAndLogInClient(login: login);
            var addGoalResponse = await AddGoal("title", "description", DateTime.UtcNow.AddDays(7));
            var goalId = addGoalResponse.Headers.Location!.ToString().Split('/').Last();
            //Act
            var deleteGoalResponse = await DeleteGoal(goalId);
            //Assert
            deleteGoalResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var goals = await FindUserGoals(login);
            goals.Should().BeEmpty();
        }
        [Fact]
        public async Task Delete_goal_fails_when_goal_is_not_exist()
        {
            //Arrange
            string login = "user";
            await RegisterAndLogInClient(login: login);
            //Act
            var deleteGoalResponse = await DeleteGoal(Guid.NewGuid().ToString());
            //Assert
            deleteGoalResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task Update_goal_pass()
        {
            //Arrange
            string login = "user";
            await RegisterAndLogInClient(login: login);
            var addGoalResponse = await AddGoal("title", "description", DateTime.UtcNow.AddDays(7));
            var goalId = addGoalResponse.Headers.Location!.ToString().Split('/').Last();
            var newDeadline = DateTime.UtcNow.AddMonths(1);
            var goalRequest = new GoalRequest("new title", "new description", newDeadline);
            //Act
            var updateGoalResponse = await UpdateGoal(goalId, goalRequest);
            //Assert
            updateGoalResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseGoal = await updateGoalResponse.Content.ReadFromJsonAsync<GoalResponse>(TestContext.Current.CancellationToken);
            responseGoal.Should().NotBeNull();
            //responseGoal.Id.ToString().Should().Be(goalId); // Id = 00000000-0000-0000-0000-000000000000
            responseGoal.Title.Should().Be("new title");
            responseGoal.Description.Should().Be("new description");
            responseGoal.Deadline.Should().Be(newDeadline);

            var dbGoals = await FindUserGoals(login);
            dbGoals.Count.Should().Be(1);
            var dbGoal = dbGoals.First();
            dbGoal.Id.ToString().Should().Be(goalId);
            dbGoal.Title.Should().Be("new title");
            dbGoal.Description.Should().Be("new description");
            dbGoal.Deadline.Should().BeCloseTo(newDeadline, 1.Milliseconds());
        }
        [Fact]
        public async Task Update_goal_fails_when_goal_is_not_exist()
        {
            //Arrange
            string login = "user";
            await RegisterAndLogInClient(login: login);
            var newDeadline = DateTime.UtcNow.AddMonths(1);
            var goalRequest = new GoalRequest("new title", "new description", newDeadline);
            //Act
            var updateGoalResponse = await UpdateGoal(Guid.NewGuid().ToString(), goalRequest);
            //Assert
            updateGoalResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var dbGoals = await FindUserGoals(login);
            dbGoals.Should().BeEmpty();
        }
        [Fact]
        public async Task Update_goal_fails_when_new_title_is_empty()
        {
            //Arrange
            string login = "user";
            await RegisterAndLogInClient(login: login);
            var deadline = DateTime.UtcNow.AddDays(7);
            var addGoalResponse = await AddGoal("title", "description", deadline);
            var goalId = addGoalResponse.Headers.Location!.ToString().Split('/').Last();
            var goalRequest = new GoalRequest("", "description", deadline);
            //Act
            var updateGoalResponse = await UpdateGoal(goalId, goalRequest);
            //Assert
            updateGoalResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var dbGoals = await FindUserGoals(login);
            dbGoals.Count.Should().Be(1);
            var dbGoal = dbGoals.First();
            dbGoal.Id.ToString().Should().Be(goalId);
            dbGoal.Title.Should().Be("title");
        }
        [Fact]
        public async Task Update_goal_fails_when_new_deadline_is_expired()
        {
            //Arrange
            string login = "user";
            await RegisterAndLogInClient(login: login);
            var oldDeadline = DateTime.UtcNow.AddDays(7);
            var addGoalResponse = await AddGoal("title", "description", oldDeadline);
            var goalId = addGoalResponse.Headers.Location!.ToString().Split('/').Last();
            var newDeadline = DateTime.UtcNow.AddMonths(-1);
            var goalRequest = new GoalRequest("title", "description", newDeadline);
            //Act
            var updateGoalResponse = await UpdateGoal(goalId, goalRequest);
            //Assert
            updateGoalResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var dbGoals = await FindUserGoals(login);
            dbGoals.Count.Should().Be(1);
            var dbGoal = dbGoals.First();
            dbGoal.Id.ToString().Should().Be(goalId);
            dbGoal.Deadline.Should().BeCloseTo(oldDeadline, 1.Milliseconds());
        }
    }
}