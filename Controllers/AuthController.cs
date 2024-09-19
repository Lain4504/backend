using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using BackEnd.Service;

namespace BackEnd.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] DTO.Request.RegisterRequest registerRequest)
        {
            // Kiểm tra xem email đã tồn tại chưa
            var existingUser = await _userService.GetUserByEmailAsync(registerRequest.email);
            if (existingUser != null)
            {
                return BadRequest("Email already in use.");
            }

            // Đăng ký người dùng
            var user = await _userService.RegisterAsync(registerRequest.email, registerRequest.password);

            if (user == null)
            {
                // Nếu việc đăng ký người dùng không thành công
                return StatusCode(500, "An error occurred while registering the user.");
            }

            // Trả về thông báo thành công với thông tin người dùng mới
            return CreatedAtAction(nameof(Register), new { Id = user.Id }, new { id = user.Id, email = user.Email });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] DTO.Request.LoginRequest loginRequest)
        {
            // Kiểm tra đầu vào rỗng
            if (string.IsNullOrEmpty(loginRequest.email) || string.IsNullOrEmpty(loginRequest.password))
            {
                return BadRequest("Email và mật khẩu không được để trống.");
            }

            try
            {
                // Xác thực người dùng bằng email và mật khẩu gốc (không mã hóa)
                var existingUser = await _userService.AuthenticateAsync(loginRequest.email, loginRequest.password);
                if (existingUser == null)
                {
                    // Trả về lỗi nếu thông tin đăng nhập không hợp lệ
                    return Unauthorized("Email hoặc mật khẩu không đúng.");
                }

                // Tạo các claim cho người dùng
                var claims = new[]
                {
                    new Claim(ClaimTypes.Email, existingUser.Email),
                    new Claim(ClaimTypes.Role, existingUser.Role?.ToString())
                };


                // Tạo đối tượng ClaimsIdentity với các claim đã tạo
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                // Thiết lập cookie phiên làm việc cho người dùng
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                // Trả về thông báo thành công
                return Ok(new { message = "Đăng nhập thành công.", user = existingUser });
            }
            catch (Exception ex)
            {
                // Xử lý các lỗi khác
                return StatusCode(500, "Đã xảy ra lỗi trong quá trình đăng nhập.");
            }
        }

    }
}
