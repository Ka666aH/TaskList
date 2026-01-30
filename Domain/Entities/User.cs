namespace Domain.Entities
{
    public class User
    {
        public string Login { get; init; }
        public string HashedPassword { get; private set; }
        //private List<Goal> _goals = [];
        //public IReadOnlyList<Goal> Goals => _goals.AsReadOnly();
#pragma warning disable CS8618 
        public User(string login, string hashedPassword)
        {
            if (string.IsNullOrWhiteSpace(login)) throw new ArgumentNullException("Login can't be empty.");
            Login = login.Trim();

            SetHashedPassword(hashedPassword);
        }
        public void SetHashedPassword(string hashedPassword)
        {
            if (string.IsNullOrWhiteSpace(hashedPassword)) throw new ArgumentNullException("Hashed password can't be empty.");
            HashedPassword = hashedPassword;
        }
        //public void AddGoal(Goal goal) => _goals.Add(goal);
        //public void RemoveGoal(Goal goal) => _goals.Remove(goal);

        private User() { }
#pragma warning restore CS8618 
    }
}