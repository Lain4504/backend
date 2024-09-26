using System.Security.Claims;
using System.Threading.Tasks;
using BackEnd.Models;
namespace BackEnd.Service
{
    public interface IUserService
    {
        Task<User> AuthenticateAsync(string Email, string Password);
        Task<User> RegisterAsync(string Email, string Password);
        Task<User> GetUserByEmailAsync(String Email);
        string GenerateJwtToken(string email);
        ClaimsPrincipal ValidateJwtToken(string token);
        Task<User> ActivateAccountAsync(string email);
    }
}
