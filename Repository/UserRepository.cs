using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BackEnd.Model;

namespace BackEnd.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly BookStoreContext _context;

        public UserRepository(BookStoreContext context)
        {
            _context = context;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var user = await _context.user
                .Where(u => u.email == email)
                .Select(u => new
                {
                    full_name = u.full_name,
                    email = u.email,
                    password = u.password,
                    role = u.role
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
                email = user.email,
                password = user.password,
                role = user.role
            };
        }



        public async Task AddAsync(User user)
        {
            await _context.user.AddAsync(user);
            await _context.SaveChangesAsync();
        }

    }
}
