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
            // Danh sách các endpoint cần kiểm tra token
            var protectedPaths = new[]
            {
                "/activate",
                "/reset-password",
                "/change-password",
                "/api/collection",
                "/api/collection/{id}"
            };

            // Kiểm tra nếu đường dẫn hiện tại có trong danh sách các endpoint bảo vệ
            if (protectedPaths.Any(path => context.Request.Path.Value.StartsWith(path)))
            {
                // Tạo scope mới để lấy IJwtService
                using (var scope = context.RequestServices.CreateScope())
                {
                    var jwtService = scope.ServiceProvider.GetRequiredService<IJwtService>();

                    var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                    if (token != null)
                    {
                        var principal = jwtService.ValidateJwtToken(token);
                        if (principal == null)
                        {
                            context.Response.StatusCode = 401; // Unauthorized
                            await context.Response.WriteAsync("{\"message\":\"Token không hợp lệ hoặc đã hết hạn\"}");
                            return;
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = 401; // Unauthorized
                        await context.Response.WriteAsync("{\"message\":\"Token không tồn tại\"}");
                        return;
                    }
                }
            }

            // Nếu không có token cần kiểm tra, tiếp tục xử lý yêu cầu
            await _next(context);
        }
    }
}