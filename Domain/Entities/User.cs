using Domain.Constants;
using Domain.Exceptions;

namespace Domain.Entities
{
    public class User
    {
        public string Login { get; init; }
        public string HashedPassword { get; private set; }
        private List<Goal> _goals = [];
        public IReadOnlyList<Goal> Goals => _goals.AsReadOnly();
        public int RoleId { get; private set; }
        public Role Role { get; }
#pragma warning disable CS8618 
        public User(string login, string hashedPassword, int roleId = (int)RoleType.Client)
        {
            if (string.IsNullOrWhiteSpace(login)) throw new UserEmptyLoginException();
            Login = login.Trim();

            SetHashedPassword(hashedPassword);
            SetUserRoleId(roleId);
        }
        public void SetHashedPassword(string hashedPassword)
        {
            if (string.IsNullOrWhiteSpace(hashedPassword)) throw new UserEmptyHashedPasswordException();
            HashedPassword = hashedPassword;
        }
        public void AddGoal(Goal goal) => _goals.Add(goal);
        public void RemoveGoal(Goal goal) => _goals.Remove(goal);
        public void SetUserRoleId(int roleId) => RoleId = roleId;

        private User() { }
#pragma warning restore CS8618 
    }
}