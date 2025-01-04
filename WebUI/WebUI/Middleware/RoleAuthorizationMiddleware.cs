using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace WebUI.Middleware
{
    public class RoleAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public RoleAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower();
            var token = context.Request.Cookies["userToken"];
            var userRole = context.Request.Cookies["userRoleName"];

            // Eğer login, register veya statik dosyalara erişiliyorsa middleware'i atla
            if (path.Contains("/loginregister") || 
                path.Contains("/lib/") || 
                path.Contains("/css/") || 
                path.Contains("/js/") ||
                path == "/" ||
                path == "/home" ||
                path == "/home/index")
            {
                await _next(context);
                return;
            }

            // Token yoksa login sayfasına yönlendir
            if (string.IsNullOrEmpty(token))
            {
                if (!path.Contains("/loginregister"))
                {
                    context.Response.Redirect("/LoginRegister/Login");
                    return;
                }
            }

            // Role göre sayfa erişim kontrolü
            if (path.Contains("/admin/") && userRole != "Admin")
            {
                context.Response.Redirect("/Home/Error404");
                return;
            }
            else if (path.Contains("/user/") && userRole != "User")
            {
                context.Response.Redirect("/Home/Error404");
                return;
            }
            else if (path.Contains("/doctor/") && userRole != "Doctor")
            {
                context.Response.Redirect("/Home/Error404");
                return;
            }

            await _next(context);
        }
    }

    public static class RoleAuthorizationMiddlewareExtensions
    {
        public static IApplicationBuilder UseRoleAuthorization(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RoleAuthorizationMiddleware>();
        }
    }
} 