using System.Threading.Tasks;
using BackEnd.Models;

namespace BackEnd.Repository
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task AddAsync(User user);
        Task UpdateUserAsync(User user);
    }
}
