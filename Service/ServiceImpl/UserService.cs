using System.Threading.Tasks;
using BackEnd.Models;
using BackEnd.Repository;
using BCrypt.Net;
namespace BackEnd.Service.ServiceImpl
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> AuthenticateAsync(string Email, string Password)
        {
            // Lấy người dùng từ cơ sở dữ liệu theo email
            var user = await _userRepository.GetByEmailAsync(Email);

            // Kiểm tra nếu không tìm thấy người dùng hoặc mật khẩu trong DB là null
            if (user == null || string.IsNullOrEmpty(user.Password))
            {
                return null; // Trả về null nếu không tìm thấy hoặc mật khẩu rỗng
            }

            // Giải mã mật khẩu từ cơ sở dữ liệu
            string decryptedPassword;
            try
            {
                decryptedPassword = EncryptDecryptManager.Decrypt(user.Password);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi giải mã mật khẩu
                Console.WriteLine($"Error decrypting password: {ex.Message}");
                return null;
            }

            // So sánh mật khẩu đã nhập với mật khẩu đã giải mã
            if (Password == decryptedPassword)
            {
                // Mật khẩu hợp lệ, trả về người dùng
                return user;
            }

            // Mật khẩu không hợp lệ
            return null;
        }


        public async Task<User> RegisterAsync(string Email, string Password)
        {
            try
            {
                // Mã hóa mật khẩu
                string hashedPassword = EncryptDecryptManager.Encrypt(Password);

                var user = new User
                {
                    Email = Email,
                    Password = hashedPassword,
                    Role = "0"
                };

                // Lưu người dùng vào cơ sở dữ liệu
                await _userRepository.AddAsync(user);

                // Trả về người dùng đã được lưu
                return user;
            }
            catch
            {
                // Ném lỗi để xử lý ở nơi gọi phương thức này
                throw;
            }
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            // Kiểm tra nếu không tìm thấy người dùng hoặc mật khẩu trong DB là null
            if (user == null)
            {
                return null; // Trả về null nếu không tìm thấy hoặc mật khẩu rỗng
            }
            return user;
        }
    }
}
