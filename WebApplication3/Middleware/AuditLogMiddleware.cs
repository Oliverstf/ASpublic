using System.Security.Claims;
using WebApplication3.Model;
using WebApplication3.Services;

namespace WebApplication3.Middleware
{
    public class AuditLogMiddleware
    {
        private readonly RequestDelegate _next;

        public AuditLogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IAuditLogService auditLogService)
        {
            //var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //var action = context.Request.Method + " " + context.Request.Path;

            //var log = new AuditLog
            //{
            //    UserId = userId,
            //    Action = action,
            //    Timestamp = DateTime.UtcNow
            //};

            //await auditLogService.LogAsync(log);

            await _next(context);
        }
    }
}
