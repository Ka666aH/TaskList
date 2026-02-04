namespace Presentation.Cookies
{
    public static class AuthCookieHelper
    {
        public const string Token = "token";
        public static void SetAuthCookie(this HttpResponse response, string token)
        {
            response.Cookies.Append(Token, token);
        }
        public static void DeleteAuthCookie(this HttpResponse response)
        {
            response.Cookies.Delete(Token);
        }
    }
}
