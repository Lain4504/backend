using System.Threading.Tasks;
using BackEnd.Model;
namespace BackEnd.Service
{
    public interface IUserService
    {
        Task<User> AuthenticateAsync(string email, string password);
        Task<User> RegisterAsync(string email, string password);
        Task<User> GetUserByEmailAsync(String email);
    }
}
