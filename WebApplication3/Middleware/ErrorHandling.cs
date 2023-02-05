using Microsoft.AspNetCore.Diagnostics;

namespace WebApplication3.Middleware
{
    public static class ErrorHandlingMiddleware
    {
        public static void UseErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "text/html";

                    var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (exceptionHandlerFeature != null)
                    {
                        var exception = exceptionHandlerFeature.Error;

                        await context.Response.WriteAsync($"<html><body><h1>Error: {exception.Message}</h1></body></html>");
                    }
                });
            });
        }
    }
}
