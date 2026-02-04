using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure.Token.JWT
{
    public static class JWTKey
    {
        private static readonly string _key;
        private static readonly SymmetricSecurityKey _instance;
        public static SymmetricSecurityKey Instance => _instance;

        static JWTKey()
        {
            _key = Environment.GetEnvironmentVariable("JWT_KEY") ?? throw new InvalidOperationException("JWT key not found.");
            _instance = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        }
    }
}
