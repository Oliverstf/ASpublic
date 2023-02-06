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
            if (!context.User.Identity.IsAuthenticated && (context.Request.Path.Value != "/Login") && (context.Request.Path.Value != "/Register") && (context.Request.Path.Value != "/ForgetPassword") && (context.Request.Path.Value != "/ResetPassword") && (context.Request.Path.Value != "/EmailSentConfirmed"))
            {
                context.Response.StatusCode = 401;
                context.Response.Redirect("/Login");
                return;
            }

            if (context.User.Identity.IsAuthenticated && ((context.Request.Path.Value == "/Login") || (context.Request.Path.Value == "/Register") || (context.Request.Path.Value == "/ForgetPassword") || (context.Request.Path.Value == "/ResetPassword") || (context.Request.Path.Value == "/EmailSentConfirmed")))
            {
                context.Response.Redirect("/");
                return;
            }

            await _next(context);
        }
    }
}
