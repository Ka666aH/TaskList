using Domain.Constants;
using Domain.Entities;
using FluentAssertions;
using Presentation.DTO;
using System.Net;
using System.Net.Http.Json;
namespace TaskList.Tests
{
    [Collection("Integration")]
    public class AdminControllerTests : AdminControllerTestsBase
    {
        [Fact]
        public async Task Change_role_from_client_to_admin_pass()
        {
            //Arrange
            var login = "user";
            await Register(login: login);
            await LogIn("admin", "admin");
            //Act
            var changeRoleResponse = await ChangeRole(login, RoleType.Admin);
            //Assert
            changeRoleResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var user = await FindUserByLogin(login);
            user.Should().NotBeNull();
            user.RoleId.Should().Be((int)RoleType.Admin);
        }
        [Fact]
        public async Task Change_role_from_admin_to_client_pass()
        {
            //Arrange
            var login = "user";
            await Register(login: login);
            await LogIn("admin", "admin");
            await ChangeRole(login, RoleType.Admin);
            //Act
            var changeRoleResponse = await ChangeRole(login, RoleType.Client);
            //Assert
            changeRoleResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var user = await FindUserByLogin(login);
            user.Should().NotBeNull();
            user.RoleId.Should().Be((int)RoleType.Client);
        }
        [Fact]
        public async Task Change_role_from_default_admin_to_client_fails()
        {
            //Arrange
            var login = "admin";
            await LogIn("admin", "admin");
            //Act
            var changeRoleResponse = await ChangeRole(login, RoleType.Client);
            //Assert
            changeRoleResponse.StatusCode.Should().Be(HttpStatusCode.Forbidden);
            var user = await FindUserByLogin(login);
            user.Should().NotBeNull();
            user.RoleId.Should().Be((int)RoleType.Admin);
        }
        [Fact]
        public async Task Change_role_from_client_to_client_pass()
        {
            //Arrange
            var login = "user";
            await Register(login);
            await LogIn("admin", "admin");
            //Act
            var changeRoleResponse = await ChangeRole(login, RoleType.Client);
            //Assert
            changeRoleResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var user = await FindUserByLogin(login);
            user.Should().NotBeNull();
            user.RoleId.Should().Be((int)RoleType.Client);
        }
        [Fact]
        public async Task Change_role_from_admin_to_admin_pass()
        {
            //Arrange
            var login = "user";
            await Register(login);
            await LogIn("admin", "admin");
            await ChangeRole(login, RoleType.Admin);
            //Act
            var changeRoleResponse = await ChangeRole(login, RoleType.Admin);
            //Assert
            changeRoleResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var user = await FindUserByLogin(login);
            user.Should().NotBeNull();
            user.RoleId.Should().Be((int)RoleType.Admin);
        }
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        public async Task Get_users_amount_pass(int amount)
        {
            //Arrange
            for (int i = 0; i < amount; i++)
            {
                await Register($"user{i}");
            }
            await LogIn("admin", "admin");
            //Act
            var getUsersAmountResponse = await GetUsersAmount();
            //Assert
            getUsersAmountResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var usersAmount = await getUsersAmountResponse.Content.ReadFromJsonAsync<UsersAmountResponse>(TestContext.Current.CancellationToken);
            usersAmount.Should().NotBeNull();
            usersAmount.Amount.Should().Be(++amount);
        }
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        public async Task Get_goals_amount_pass(int amount)
        {
            //Arrange
            await RegisterAndLogInClient();
            var tasks = Enumerable.Range(0, amount).Select(_ => AddGoal());
            await Task.WhenAll(tasks);
            await LogOut();
            await LogIn("admin", "admin");
            await AddGoal();
            //Act
            var getGoalsAmountResponse = await GetGoalsAmount();
            //Assert
            getGoalsAmountResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var goalsAmount = await getGoalsAmountResponse.Content.ReadFromJsonAsync<GoalsAmountResponse>(TestContext.Current.CancellationToken);
            goalsAmount.Should().NotBeNull();
            goalsAmount.Amount.Should().Be(++amount);
        }
        [Theory]
        [MemberData(nameof(GetUsersPageData))]
        public async Task Get_users_page_pass(int amount, int pageSize, int pageNumber, List<UserResponse> expected)
        {
            //Arrange
            for (int i = 0; i < amount; i++)
            {
                await Register($"user{i}");
            }
            await LogIn("admin", "admin");
            //Act
            var getUsersPageResponse = await GetUsersPage(pageSize, pageNumber);
            //Assert
            getUsersPageResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var usersPage = await getUsersPageResponse.Content.ReadFromJsonAsync<List<UserResponse>>(TestContext.Current.CancellationToken);
            usersPage.Should().NotBeNull();
            usersPage.Should().BeEqualTo(expected);
        }
        public static TheoryData<int, int, int, List<UserResponse>> GetUsersPageData()
        {
            return new TheoryData<int, int, int, List<UserResponse>>
            {
                {0,0,0, [new UserResponse("admin")] },
                {1,0,0, [new UserResponse("admin"), new UserResponse("user0")] },
                {1,1,1, [new UserResponse("admin")] },
                {1,1,2, [new UserResponse("user0")] },
                {3,2,1, [new UserResponse("admin"), new UserResponse("user0")] },
                {3,2,2, [new UserResponse("user1"), new UserResponse("user2")] },
                {3,2,3, [] },
            };
        }
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Get_users_page_fails_when_page_size_is_invalid(int pageSize)
        {
            //Arrange
            await LogIn("admin", "admin");
            //Act
            var getUsersPageResponse = await GetUsersPage(pageSize, 1);
            //Assert
            getUsersPageResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Get_users_page_fails_when_page_number_is_invalid(int pageNumber)
        {
            //Arrange
            await LogIn("admin", "admin");
            //Act
            var getUsersPageResponse = await GetUsersPage(1, pageNumber);
            //Assert
            getUsersPageResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        public async Task Get_user_goals_amount_pass(int amount)
        {
            //Arrange
            string login = "user";
            await RegisterAndLogInClient(login: login);
            var tasks = Enumerable.Range(0, amount).Select(_ => AddGoal());
            await Task.WhenAll(tasks);
            await LogOut();
            await LogIn("admin", "admin");
            //Act
            var getUserGoalsAmountResponse = await GetUserGoalsAmount(login);
            //Assert
            getUserGoalsAmountResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var goalsAmount = await getUserGoalsAmountResponse.Content.ReadFromJsonAsync<GoalsAmountResponse>(TestContext.Current.CancellationToken);
            goalsAmount.Should().NotBeNull();
            goalsAmount.Amount.Should().Be(amount);
        }
    }
}