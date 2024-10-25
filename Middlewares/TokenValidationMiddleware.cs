using BackEnd.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Middlewares
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var protectedPaths = new[]
            {
            "/activate",
            "/reset-password",
            "/change-password",
            "/api/collection/create",
            "/api/collection/delete/",
            "/api/collection/update/",
            };

            if (protectedPaths.Any(path => context.Request.Path.Value.StartsWith(path)))
            {
                using (var scope = context.RequestServices.CreateScope())
                {
                    var jwtService = scope.ServiceProvider.GetRequiredService<IJwtService>();
                    var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                    if (token == null)
                    {
                        await HandleUnauthorizedResponse(context, "Token không tồn tại");
                        return;
                    }

                    var principal = jwtService.ValidateJwtToken(token);
                    if (principal == null)
                    {
                        await HandleUnauthorizedResponse(context, "Token không hợp lệ hoặc đã hết hạn");
                        return;
                    }
                }
            }

            await _next(context);
        }

        private async Task HandleUnauthorizedResponse(HttpContext context, string message)
        {
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync($"{{\"message\":\"{message}\"}}");
        }

    }
}