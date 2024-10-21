using System.Security.Claims;

namespace BackEnd.Service
{
    public interface IJwtService
    {
        string GenerateJwtToken(string email, long id, string role);
        ClaimsPrincipal ValidateJwtToken(string token);
        string GenerateRefreshToken();
    }
}
