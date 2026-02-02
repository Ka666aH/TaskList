namespace Application.Interfaces.RepositoryInterfaces
{
    public interface IPasswordEncrypterRepository
    {
        string Encrypt(string password);
        bool Verify(string password, string hashedPassword);
    }
}
