using Application.Interfaces.ServiceInterfaces;

namespace Application.Services
{
    public class CacheKeyService : ICacheKeyService
    {
        public string GetGoalsAmountKey() => "goals_amount";
        public string GetUserGoalsAmountKey(string login) => $"{login}_goals_amount";
        public string GetUsersAmountKey() => "users_amount";
        public string GetUserWithGoalsKey(string login) => $"{login}_user_with_goals";

    }
}
