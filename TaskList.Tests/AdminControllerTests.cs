using Domain.Constants;
using FluentAssertions;
using System.Net;
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
            changeRoleResponse.StatusCode.Should().Be(HttpStatusCode.OK);
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
            changeRoleResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var user = await FindUserByLogin(login);
            user.Should().NotBeNull();
            user.RoleId.Should().Be((int)RoleType.Admin);
        }
    }
}