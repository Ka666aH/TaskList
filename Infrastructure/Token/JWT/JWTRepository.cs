using Application.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Infrastructure.Token.JWT
{
    public class JWTRepository : ITokenRepository
    {

        public string GenerateToken(User user)
        {
            var sc = new SigningCredentials(JWTKey.Instance, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: [new Claim(Claims.Login, user.Login)],
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: sc
                );

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenValue;
        }
    }
}
