using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BackEnd.Service.ServiceImpl
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Hàm tạo JWT token
        public string GenerateJwtToken(string email, long id, string role)
        {
            // Tạo danh sách các claim cho token
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "JwtSubject"),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // ID duy nhất cho token
                new Claim(ClaimTypes.NameIdentifier, id.ToString()), // ID người dùng
                new Claim(ClaimTypes.Role, role) // Vai trò của người dùng
            };

            // Lấy khóa bí mật từ cấu hình
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("x&-8m(6TQd<f`v'G.KY#7:3*PgXb2se!"));

            // Tạo đối tượng SigningCredentials với thuật toán HmacSha256
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Tạo JWT token
            var token = new JwtSecurityToken(
                issuer: "JwtIssuer", // Issuer từ appsettings
                audience: "JwtAudience", // Audience từ appsettings
                claims: claims, // Các claim cho token
                expires: DateTime.Now.AddMinutes(30), // Token hết hạn sau 30 phút
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
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return claimsPrincipal;
            }
            catch
            {
                return null;
            }
        }
    }
}
