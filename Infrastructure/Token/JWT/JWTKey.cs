using Domain.Exceptions;
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
            _key = Environment.GetEnvironmentVariable("JWT_KEY") ?? throw new JWTKeyNotFountException();
            _instance = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        }
    }
    public class JWTKeyNotFountException : EnvException
    {
        private const string _code = "JWT_KEY_NOT_FOUND";
        private const string _message = "JWT key not found.";
        public JWTKeyNotFountException() : base(_code, _message) { }
    }
}