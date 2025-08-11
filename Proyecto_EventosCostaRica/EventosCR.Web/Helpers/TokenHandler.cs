using Microsoft.AspNetCore.Http;

namespace EventosCR.Web.Helpers
{
    public static class TokenHandler
    {
        private const string TokenKey = "AuthToken";

        public static void SaveToken(HttpContext context, string token)
        {
            context.Response.Cookies.Append(TokenKey, token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // true si usas HTTPS
                SameSite = SameSiteMode.Strict
            });
        }

        public static string? GetToken(HttpContext context)
        {
            context.Request.Cookies.TryGetValue(TokenKey, out var token);
            return token;
        }

        public static void RemoveToken(HttpContext context)
        {
            context.Response.Cookies.Delete(TokenKey);
        }
    }
}
