using Application.Interfaces.RepositoryInterfaces;

namespace Infrastructure.PasswordEncrypter
{
    public class BCryptRepository : IPasswordEncrypterRepository
    {
        public string Encrypt(string password) => BCrypt.Net.BCrypt.EnhancedHashPassword(password);
        public bool Verify(string password, string hashedPassword) => BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
    }
}
