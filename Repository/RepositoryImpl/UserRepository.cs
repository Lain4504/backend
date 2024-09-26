using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BackEnd.Models;

namespace BackEnd.Repository.RepositoryImpl
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
                    u.FullName,
                    u.Email,
                    u.Password,
                    u.Role,
                    u.State
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
                Role = user.Role,
                State = user.State
            };
        }



        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateUserAsync(User user)
        {
            // Kiểm tra xem người dùng có tồn tại không bằng Email
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (existingUser == null)
            {
                throw new Exception("User not found.");
            }

            // Cập nhật các thuộc tính cần thiết
            existingUser.State = user.State;

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();
        }

    }
}