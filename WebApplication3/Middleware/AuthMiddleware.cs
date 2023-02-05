using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace WebApplication3.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                context.Response.StatusCode = 401;
                context.Response.Headers.Add("Location", "/login");
                return;
            }

            await _next(context);
        }
    }
}
