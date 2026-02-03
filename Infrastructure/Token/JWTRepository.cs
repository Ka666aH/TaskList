using Application.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Infrastructure.Token
{
    public class JWTRepository : ITokenRepository
    {
        public string GenerateToken(User user)
        {
            var sc = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes("testkeytestkeytestkeytestkeytestkey")), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: [new Claim("login", user.Login)],
                expires: DateTime.UtcNow.AddMinutes(1),
                signingCredentials: sc
                );

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenValue;
        }
    }
}
