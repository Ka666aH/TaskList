using Domain.Entities;

namespace Application.Interfaces.RepositoryInterfaces
{
    public interface ITokenRepository
    {
        string GenerateToken(User user);
    }
}
