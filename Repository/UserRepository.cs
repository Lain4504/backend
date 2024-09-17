using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BackEnd.Models;

namespace BackEnd.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly BookStoreContext _context;

        public UserRepository(BookStoreContext context)
        {
            _context = context;
        }

        public async Task<User> GetByEmailAsync(string Email)
        {
            var user = await _context.Users
                .Where(u => u.Email == Email)
                .Select(u => new
                {
                    FullName = u.FullName,
                    Email = u.Email,
                    Password = u.Password,
                    Role = u.Role
                })
                .SingleOrDefaultAsync();

            if (user == null)
            {
                // Xử lý khi không tìm thấy người dùng
                return null;
            }

            // Xử lý các trường có thể null
            return new User
            {
                Email = user.Email,
                Password = user.Password,
                Role = user.Role
            };
        }



        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

    }
}
