using FluentAssertions;
using System.Net;
namespace TaskList.Tests
{
    [Collection("Integration")]
    public class UserControllerTests : UserControllerTestsBase
    {
        [Fact]
        public async Task Delete_account_pass()
        {
            //Arrange
            var login = "user";
            await CreateAndAutorizeClient(login);
            //Act
            var deleteResponse = await DeleteAccount();
            //Assert
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
            (await FindUserByLogin(login)).Should().BeNull();
            (await CountUsersInDatabase()).Should().Be(1);
        }
        [Fact]
        public async Task Delete_account_fails_when_it_is_default_admin()
        {
            //Arrange
            string login = "admin";
            string password = "admin";
            await LogIn(login, password);
            //Act
            var deleteResponse = await DeleteAccount();
            //Assert
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.Forbidden);
            (await FindUserByLogin(login)).Should().NotBeNull();
            (await CountUsersInDatabase()).Should().Be(1);
        }
        [Fact]
        public async Task Change_password_pass()
        {
            //Arrange
            var login = "user";
            var password = "password";
            var newPassword = "newpassword";
            await CreateAndAutorizeClient(login, password);

            //Act
            var changeResponse = await ChangePassword(newPassword);

            //Assert
            changeResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var user = await FindUserByLogin(login);
            user.Should().NotBeNull();
            VerifyPassword(newPassword, user.HashedPassword).Should().BeTrue();
        }
        [Fact]
        public async Task Change_password_fails_when_new_password_is_the_same_as_old()
        {
            //Arrange
            var login = "user";
            var password = "password";
            await CreateAndAutorizeClient(login, password);

            //Act
            var changeResponse = await ChangePassword(password);

            //Assert
            changeResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
