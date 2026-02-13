namespace Application.Interfaces.ServiceInterfaces
{
    public interface ICacheKeyService
    {
        string GetUsersAmountKey();
        string GetGoalsAmountKey();
        string GetUserGoalsAmountKey(string login);
        string GetUserWithGoalsKey(string login);
    }
}
