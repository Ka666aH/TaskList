using Application.Interfaces.RepositoryInterfaces;
using FluentAssertions;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Presentation.DTO;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
namespace TaskList.Tests
{
    public class AuthControllerTests : IntergrationTestsBase
    {
        private readonly string _registerUrl = "/auth/reg";
        private readonly string _logInUrl = "/auth/login";
        private readonly string _logOutUrl = "/auth/logout";

        protected EFCDbContext _db => _scope.ServiceProvider.GetRequiredService<EFCDbContext>();
        protected IPasswordEncrypterRepository _passwordEncrypter => _scope.ServiceProvider.GetRequiredService<IPasswordEncrypterRepository>();

        [Fact]
        public async Task Register_pass_when_avaliable_login_and_nonempty_password()
        {
            //Arrange
            var userRequest = new UserRequest("user", "password");

            //Act
            var response = await _httpClient.PostAsJsonAsync(_registerUrl, userRequest, TestContext.Current.CancellationToken);

            //Assert
            //Response
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            //DB state
            var user = await _db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                x.Login == userRequest.Login,
                TestContext.Current.CancellationToken);
            //User exist
            user.Should().NotBeNull();
            user.Login.Should().Be(userRequest.Login);
            //Correct password hash
            bool verification = _passwordEncrypter.Verify(userRequest.Password, user.HashedPassword);
            verification.Should().BeTrue();
        }
        [Fact]
        public async Task Register_fails_when_existing_login()
        {
            //Arrange
            var userRequest = new UserRequest("user", "password");

            var firstResponse = await _httpClient.PostAsJsonAsync(
                _registerUrl,
                userRequest,
                TestContext.Current.CancellationToken);

            //Act
            var result = await _httpClient.PostAsJsonAsync(
                _registerUrl,
                userRequest,
                TestContext.Current.CancellationToken);

            //Assert
            firstResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            result.StatusCode.Should().Be(HttpStatusCode.Conflict);

            int usersAmount = await _db.Users.CountAsync(TestContext.Current.CancellationToken);
            usersAmount.Should().Be(2);
        }
        [Fact]
        public async Task Register_fails_when_login_admin()
        {
            //Arrange
            var userRequest = new UserRequest("admin", "password");

            //Act
            var registerResponse = await _httpClient.PostAsJsonAsync(_registerUrl, userRequest, TestContext.Current.CancellationToken);

            //Assert
            registerResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);

            int usersAmount = await _db.Users.CountAsync(TestContext.Current.CancellationToken);
            usersAmount.Should().Be(1);
        }
        [Theory]
        [InlineData ("")]
        [InlineData (" ")]
        [InlineData ("  ")]
        public async Task Register_fails_when_empty_login(string login)
        {
            //Arrange
            var userRequest = new UserRequest(login, "password");

            //Act
            var registerResponse = await _httpClient.PostAsJsonAsync(_registerUrl, userRequest, TestContext.Current.CancellationToken);

            //Assert
            registerResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            int usersAmount = await _db.Users.CountAsync(TestContext.Current.CancellationToken);
            usersAmount.Should().Be(1);
        }
        [Theory]
        [InlineData ("")]
        [InlineData (" ")]
        [InlineData ("  ")]
        public async Task Register_fails_when_empty_password(string password)
        {
            //Arrange
            var userRequest = new UserRequest("user", password);

            //Act
            var registerResponse = await _httpClient.PostAsJsonAsync(_registerUrl, userRequest, TestContext.Current.CancellationToken);

            //Assert
            registerResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            int usersAmount = await _db.Users.CountAsync(TestContext.Current.CancellationToken);
            usersAmount.Should().Be(1);
        }
        [Fact]
        public async Task Log_in_pass_with_valid_data()
        {
            //Регистрация
            //Arrange
            var userRequest = new UserRequest("user", "password");
            //Act
            var regResponse = await _httpClient.PostAsJsonAsync(_registerUrl, userRequest, TestContext.Current.CancellationToken);
            //Assert
            regResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            //Act
            var loginResponse = await _httpClient.PostAsJsonAsync(_logInUrl, userRequest, TestContext.Current.CancellationToken);
            //Assert
            loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            //Cookie
            loginResponse.Headers.Should().ContainKey("Set-Cookie");
            var cookies = loginResponse.Headers.GetValues("Set-Cookie").ToList();
            cookies.Count.Should().Be(1);
            cookies[0].StartsWith("token=").Should().BeTrue();
        }
        [Fact]
        public async Task Log_in_fails_when_user_not_found()
        {
            //Arrange
            var userRequest = new UserRequest("user", "password");
            //Act
            var loginResponse = await _httpClient.PostAsJsonAsync(_logInUrl, userRequest, TestContext.Current.CancellationToken);
            //Assert
            loginResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
            loginResponse.Headers.Should().NotContainKey("Set-Cookie");
        }
        [Fact]
        public async Task Log_in_fails_when_password_incorrect()
        {
            //Регистрация
            //Arrange
            var userRequest = new UserRequest("user", "password");
            //Act
            var regResponse = await _httpClient.PostAsJsonAsync(_registerUrl, userRequest, TestContext.Current.CancellationToken);
            //Assert
            regResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            //Arrange
            var userRequestWithIncorrectPassword = new UserRequest("user", "incorrect");
            //Act
            var loginResponse = await _httpClient.PostAsJsonAsync(_logInUrl, userRequestWithIncorrectPassword, TestContext.Current.CancellationToken);
            //Assert
            loginResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            loginResponse.Headers.Should().NotContainKey("Set-Cookie");
        }
        [Fact]
        public async Task Log_out_pass()
        {
            //Регистрация
            //Arrange
            var userRequest = new UserRequest("user", "password");
            //Act
            var regResponse = await _httpClient.PostAsJsonAsync(_registerUrl, userRequest, TestContext.Current.CancellationToken);
            //Assert
            regResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            //Act
            var loginResponse = await _httpClient.PostAsJsonAsync(_logInUrl, userRequest, TestContext.Current.CancellationToken);
            //Assert
            loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            //// 1. Логин → получаем токен из ответа
            var token = loginResponse.Headers
                .GetValues("Set-Cookie")
                .First(c => c.StartsWith("token="))
                .Split(';')
                .First()
                .Substring("token=".Length);


            //// 2. Явно добавляем заголовок для следующих запросов
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            //Act
            var logoutResponse = await _httpClient.PostAsync(_logOutUrl, null, TestContext.Current.CancellationToken);
            //Assert
            logoutResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var cookies = logoutResponse.Headers.GetValues("Set-Cookie").ToList();
            cookies.Count.Should().Be(1);
            cookies[0].StartsWith("token=").Should().BeTrue();
            cookies[0].Should().Contain("expires=Thu, 01 Jan 1970");
        }
    }
}