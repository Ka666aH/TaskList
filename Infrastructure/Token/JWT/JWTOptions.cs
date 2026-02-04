using Infrastructure.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure.Token.JWT
{
    public static class JWTOptions
    {
        public static string Key() => Environment.GetEnvironmentVariable("JWT_KEY") ?? throw new Exception("JWT key not found.");
        public static void Configure(JwtBearerOptions options)
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key()))
            };
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    if (context.Request.Cookies.TryGetValue(CookieHelper.AuthCookieName, out var token)) context.Token = token;
                    return Task.CompletedTask;
                }
            };
        }
    }
}