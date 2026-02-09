using Microsoft.AspNetCore.Authorization;

namespace Presentation.Options
{
    public static class Policies
    {
        public const string RequireLogin = "LoggedIn";
    }
}
