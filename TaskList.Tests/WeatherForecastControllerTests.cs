using Domain.Entities;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace TaskList.Tests
{
    public class WeatherForecastControllerTests : IntergrationTestsBase
    {
        [Fact]
        public async Task Unauthorized_user_get_weather_forecast_pass()
        {
            //Act
            var getWeatherForecastResponse = await _httpClient.GetAsync("/WeatherForecast", TestContext.Current.CancellationToken);
            //Assert
            getWeatherForecastResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var forecasts = await getWeatherForecastResponse.Content.ReadFromJsonAsync<List<WeatherForecast>>(TestContext.Current.CancellationToken);
            forecasts.Should().NotBeNull();
            forecasts.Count.Should().Be(5);
            forecasts.Should().OnlyContain(f =>
            f.Date >= DateOnly.FromDateTime(DateTime.Now) && f.Date <= DateOnly.FromDateTime(DateTime.Now.AddDays(5)) &&
            f.TemperatureC >= -20 && f.TemperatureC < 55 &&
            !string.IsNullOrEmpty(f.Summary)
            );
        }
        [Fact]
        public async Task Authorized_user_get_weather_forecast_pass()
        {
            //Act
            await RegisterAndLogInClient();
            var getWeatherForecastResponse = await _httpClient.GetAsync("/WeatherForecast", TestContext.Current.CancellationToken);
            //Assert
            getWeatherForecastResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var forecasts = await getWeatherForecastResponse.Content.ReadFromJsonAsync<List<WeatherForecast>>(TestContext.Current.CancellationToken);
            forecasts.Should().NotBeNull();
            forecasts.Count.Should().Be(5);
            forecasts.Should().OnlyContain(f =>
            f.Date >= DateOnly.FromDateTime(DateTime.Now) && f.Date <= DateOnly.FromDateTime(DateTime.Now.AddDays(5)) &&
            f.TemperatureC >= -20 && f.TemperatureC < 55 &&
            !string.IsNullOrEmpty(f.Summary)
            );
        }
    }
}
