using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Infrastructure.Token;
using Infrastructure.Token.JWT;

namespace Presentation.Options
{
    public static class JWTOptions
    {
        public static void Configure(JwtBearerOptions options)
        {
            options.MapInboundClaims = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = JWTKey.Instance,
                RoleClaimType = Claims.Role,
                NameClaimType = Claims.Login
            };
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    if (context.Request.Cookies.TryGetValue(Cookies.AuthCookieHelper.Token, out var token)) context.Token = token;
                    return Task.CompletedTask;
                }
            };
        }
    }
}