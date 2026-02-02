using Application.Interfaces.RepositoryInterfaces;
using Application.Interfaces.ServiceInterfaces;
using Domain.Entities;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _ur;
        private readonly IUnitOfWork _uow;
        private readonly IPasswordEncrypterRepository _per;
        private readonly ITokenRepository _tr;

        public AuthService(IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordEncrypterRepository passwordEncrypterRepository, ITokenRepository tr)
        {
            _ur = userRepository;
            _uow = unitOfWork;
            _per = passwordEncrypterRepository;
            _tr = tr;
        }
        public async Task<bool> RegisterAsync(string login, string password, CancellationToken ct = default)
        {
            var existingUser = await _ur.GetUserAsync(login, ct);
            if (existingUser != null) throw new ArgumentException("User with this login is already exist.");

            var hashedPassword = _per.Encrypt(password);
            var user = new User(login, hashedPassword);

            await _ur.AddUserAsync(user, ct);
            return await _uow.SaveChangesAsync(ct);
        }

        public async Task<string> LoginAsync(string login, string password, CancellationToken ct = default)
        {
            var existingUser = await _ur.GetUserAsync(login, ct);
            if (existingUser == null) throw new NullReferenceException("User not found.");

            if (!_per.Verify(password, existingUser.HashedPassword)) throw new ArgumentException("Incorrect password.");
            return _tr.GenerateToken(existingUser); ;
        }
    }
}