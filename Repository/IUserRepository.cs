using System.Threading.Tasks;
using BackEnd.Model;

namespace BackEnd.Repository
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task AddAsync(User user);
    }
}
