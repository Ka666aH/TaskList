using Microsoft.AspNetCore.Http;
using System.Runtime.CompilerServices;

namespace Infrastructure.Cookies
{
    public static class CookieHelper
    {
        public const string AuthCookieName = "jwt";
        public static void SetAuthCookie(this HttpResponse response, string token)
        {
            response.Cookies.Append(AuthCookieName, token);
        }
        public static void DeleteAuthCookie(this HttpResponse response)
        {
            response.Cookies.Delete(AuthCookieName);
        }
    }
}
