using System.Threading.Tasks;
using BackEnd.DTO.Request;
using BackEnd.Models;

namespace BackEnd.Repository
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByIDAsync(long id);
        Task AddAsync(User user);
        Task UpdateUserAsync(User user);
        Task UpdateUserPassword(User user);
        Task UpdateUserProfile(UserUpdateRequest user, long id);
        Task ChangePassword(User user);

        Task<IEnumerable<User>> GetAllUsersAsync();
        Task DeleteUserAsync(long id);
    }
}
