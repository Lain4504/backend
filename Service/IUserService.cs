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
        Task<User> RegisterAsync(string Email);
        Task<User> GetUserByEmailAsync(String Email);
        Task<User> ActivateAccountAsync(string email);
        Task<bool> UpdatePassword(string email, string newPassword);
        Task<User> GetUserByIDAsync(long id);
        Task UpdateProfile(UserUpdateRequest user, long id);
        Task UpdateUserRoleAndState(User user, long id);

        Task<bool> ChangePassword(UserChangePassword userChange, string email);
        Task<IEnumerable<User>> GetAllUser();
        Task DeleteUserById(long id);
    }
}