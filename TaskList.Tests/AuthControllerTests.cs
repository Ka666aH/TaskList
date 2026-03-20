using FluentAssertions;
using System.Net;
namespace TaskList.Tests
{
    public class AuthControllerTests : AuthControllerTestsBase
    {
        [Fact]
        public async Task Register_pass_when_available_login_and_nonempty_password()
        {
            //Arrange
            var login = "user";
            var password = "password";
            //Act
            var registerResponse = await Register(login, password);

            //Assert
            //Response
            registerResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            //DB state
            var user = await FindUserByLogin(login);
            user.Should().NotBeNull();
            user.Login.Should().Be(login);
            //Correct password hash
            VerifyPassword(password, user.HashedPassword).Should().BeTrue();
        }
        [Fact]
        public async Task Register_fails_when_existing_login()
        {
            //Arrange
            var login = "user";
            var password = "password";
            await Register(login, password);

            //Act
            var registerResponse = await Register(login, password);

            //Assert
            registerResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);

            int usersAmount = await CountUsersInDatabase();
            usersAmount.Should().Be(2);
        }
        [Fact]
        public async Task Register_fails_when_login_admin()
        {
            //Arrange
            var login = "admin";
            var password = "password";

            //Act
            var registerResponse = await Register(login, password);

            //Assert
            registerResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);

            int usersAmount = await CountUsersInDatabase();
            usersAmount.Should().Be(1);
        }
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public async Task Register_fails_when_empty_login(string userLogin)
        {
            //Arrange
            string password = "password";

            //Act
            var registerResponse = await Register(userLogin, password);

            //Assert
            registerResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            int usersAmount = await CountUsersInDatabase();
            usersAmount.Should().Be(1);
        }
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public async Task Register_fails_when_empty_password(string userPassword)
        {
            //Arrange
            string login = "user";

            //Act
            var registerResponse = await Register(login, userPassword);

            //Assert
            registerResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            int usersAmount = await CountUsersInDatabase();
            usersAmount.Should().Be(1);
        }
        [Fact]
        public async Task Log_in_pass_with_valid_data()
        {
            //Регистрация
            //Arrange
            var login = "user";
            var password = "password";
            //Act
            var registerResponse = await Register(login, password);
            //Assert
            registerResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            //Act
            var loginResponse = await LogIn(login, password);
            //Assert
            loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            //Cookie
            loginResponse.Headers.Should().ContainKey("Set-Cookie");
            var cookies = loginResponse.Headers.GetValues("Set-Cookie").ToList();
            cookies.Count.Should().Be(1);
            var tokenCookie = cookies[0];
            tokenCookie.StartsWith("token=").Should().BeTrue();
        }
        [Fact]
        public async Task Log_in_fails_when_user_not_found()
        {
            //Arrange
            var login = "user";
            var password = "password";
            //Act
            var loginResponse = await LogIn(login, password);
            //Assert
            loginResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
            loginResponse.Headers.Should().NotContainKey("Set-Cookie");
        }
        [Fact]
        public async Task Log_in_fails_when_password_incorrect()
        {
            //Регистрация
            //Arrange
            var login = "user";
            var password = "password";
            var incorrectPassword = "incorrect";
            //Act
            var registerResponse = await Register(login, password);
            //Assert
            registerResponse.StatusCode.Should().Be(HttpStatusCode.Created);            
            //Act
            var loginResponse = await LogIn(login, incorrectPassword);
            //Assert
            loginResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            loginResponse.Headers.Should().NotContainKey("Set-Cookie");
        }
        [Fact]
        public async Task Log_out_pass()
        {
            //Регистрация
            //Arrange
            var login = "user";
            var password = "password";
            //Act
            var registerResponse = await Register(login, password);
            //Assert
            registerResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            //Act
            var loginResponse = await LogIn(login, password);
            //Assert
            loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            //Arrange
            //Получение токена
            var token = GetTokenFromResponse(loginResponse);
            //Установка токена
            SetTokenToHTTPClient(token);

            //Act
            var logoutResponse = await LogOut();
            //Assert
            logoutResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var cookies = logoutResponse.Headers.GetValues("Set-Cookie").ToList();
            cookies.Count.Should().Be(1);
            cookies[0].StartsWith("token=").Should().BeTrue();
            cookies[0].Should().Contain("expires=Thu, 01 Jan 1970");
        }
    }
}