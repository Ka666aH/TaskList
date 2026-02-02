namespace Application.Interfaces.ServiceInterfaces
{
    public interface IAuthService
    {
        bool Register(string login, string password);
        string Login(string login, string password);
    }
}
