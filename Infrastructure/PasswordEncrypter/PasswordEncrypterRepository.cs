using Application.Interfaces.RepositoryInterfaces;
using BCrypt.Net;

namespace Infrastructure.PasswordEncrypter
{
    internal class PasswordEncrypterRepository : IPasswordEncrypterRepository
    {
        public string Encrypt(string password) => BCrypt.Net.BCrypt.EnhancedHashPassword(password);
        public bool Verify(string password, string hashedPassword) => BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
    }
}
