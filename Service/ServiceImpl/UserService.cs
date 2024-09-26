using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BackEnd.Models;
using BackEnd.Repository;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
namespace BackEnd.Service.ServiceImpl
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;


        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
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

            // Kiểm tra trạng thái người dùng
            if (user.State != "ACTIVE") 
            {
                return null; // Trả về null nếu trạng thái không phải là active
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
                    Role = "0",
                    State ="INACTIVE"
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



        // Hàm tạo JWT token
        public string GenerateJwtToken(string email)
        {
            // Tạo danh sách các claim cho token
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // ID duy nhất cho token
        };

            // Lấy khóa bí mật từ cấu hình
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            // Tạo đối tượng SigningCredentials với thuật toán HmacSha256
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Tạo JWT token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"], // Issuer từ appsettings
                audience: _configuration["Jwt:Audience"], // Audience từ appsettings
                claims: claims, // Các claim cho token
                expires: DateTime.Now.AddDays(1), // Token hết hạn sau 1 ngày
                signingCredentials: creds); // Sử dụng thông tin SigningCredentials

            // Trả về chuỗi JWT token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public ClaimsPrincipal ValidateJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            try
            {
                var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero // Không cho phép sai lệch thời gian
                }, out SecurityToken validatedToken);

                return claimsPrincipal;
            }
            catch
            {
                return null;
            }
        }
        public async Task<User> ActivateAccountAsync(string email)
        {
            // Tìm kiếm người dùng theo email
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                return null; // Trả về null nếu không tìm thấy người dùng
            }

            // Cập nhật trạng thái tài khoản thành active
            user.State = "ACTIVE";

            // Lưu thay đổi vào cơ sở dữ liệu
            await _userRepository.UpdateUserAsync(user);

            return user; // Trả về thông tin người dùng đã được kích hoạt
        }


    }
}
