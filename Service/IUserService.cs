using System.Security.Claims;
using System.Threading.Tasks;
using BackEnd.DTO.Request;
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
        Task<bool> UpdatePassword(string email, string newPassword);
        Task UpdateProfile(UserUpdateRequest user, long id);
        Task<User> GetUserByIDAsync(long id);
        Task<bool> ChangePassword(UserChangePassword userChange, long Id);
        Task<IEnumerable<User>> GetAllUser();
        Task DeleteUserById(long id);
    }
}