using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace LostAndFoundApp.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower();
            
            // Login sayfası ve API endpoint'lerine erişim serbest
            if (path != null && (path.StartsWith("/account/login") || path.StartsWith("/api/")))
            {
                await _next(context);
                return;
            }
            
            // Diğer tüm sayfalara login zorunlu
            var userName = context.Session.GetString("UserName");
            var userRole = context.Session.GetString("UserRole");
            
            if (string.IsNullOrEmpty(userName))
            {
                // Login sayfasına yönlendir
                context.Response.Redirect("/Account/Login");
                return;
            }
            
            // Kullanıcılar sayfası sadece Admin'e özel
            if (path != null && path.StartsWith("/users") && userRole != "Admin")
            {
                // Yetkisiz erişim - Ana sayfaya yönlendir
                context.Response.Redirect("/Home/Index?error=unauthorized");
                return;
            }
            
            // Diğer tüm sayfalar için session kontrolü yapıldı, devam et
            await _next(context);
        }
    }
}