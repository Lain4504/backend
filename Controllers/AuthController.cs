using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using BackEnd.Service;
using BackEnd.DTO.Request;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity.Data;
using BackEnd.Models;

namespace BackEnd.Controllers
{
    [Route("api/user")]
    [ApiController]
    [EnableCors("AllowSpecificOrigins")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public AuthController(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
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
                return StatusCode(500, "An error occurred while registering the user.");
            }

            // Tạo JWT token kích hoạt tài khoản
            var token = _userService.GenerateJwtToken(user.Email);

            // Gửi email kích hoạt
            await _emailService.SendActivationEmail(user.Email, token);

            return Ok(new { message = "Registration successful. Please check your email to activate your account." });
        }
        [HttpPost("activate")]
        public async Task<IActionResult> ActivateAccount([FromBody] ActivateAccountRequest request)
        {
            // Kiểm tra xem token có được cung cấp không
            if (request == null || string.IsNullOrEmpty(request.Token))
            {
                return BadRequest("Token is required.");
            }

            // Xác thực token
            var claimsPrincipal = _userService.ValidateJwtToken(request.Token);

            if (claimsPrincipal == null)
            {
                return BadRequest("Invalid token.");
            }

            // Lấy email từ claim
            var email = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Invalid token.");
            }

            // Kích hoạt tài khoản người dùng
            var user = await _userService.ActivateAccountAsync(email);

            if (user == null)
            {
                return StatusCode(500, "An error occurred while activating the account.");
            }

            return Ok(new { message = "Account activated successfully." });
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
                {   new Claim(ClaimTypes.NameIdentifier, existingUser.Id.ToString()),
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
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPasword([FromBody] DTO.Request.ForgotPasswordRequest forgotRequest)
        {
            if (string.IsNullOrEmpty(forgotRequest.Email))
            {
                return BadRequest("Email không hợp lệ.");
            }
            // Gửi email với liên kết đặt lại mật khẩu chứa token
            await _emailService.SendResetPasswordEmail(forgotRequest.Email);
            return Ok("Vui lòng kiểm tra email của bạn để đặt lại mật khẩu.");
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] DTO.Request.ResetPasswordRequest resetRequest)
        {
            // Kiểm tra tính hợp lệ của token
            var claimsPrincipal = _userService.ValidateJwtToken(resetRequest.Token);
            if (claimsPrincipal == null)
            {
                return BadRequest("Token không hợp lệ hoặc đã hết hạn.");
            }

            // Lấy email từ claims (nếu bạn đã đưa email vào token)
            var email = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value;

            // Thực hiện việc đặt lại mật khẩu (giả định bạn có phương thức UpdatePassword)
            var result = await _userService.UpdatePassword(email, resetRequest.NewPassword);
            if (!result)
            {
                return BadRequest("Đặt lại mật khẩu không thành công.");
            }

            return Ok("Mật khẩu đã được đặt lại thành công.");
        }
        [HttpGet("get-profile/{id}")]
        public async Task<IActionResult> GetProfile(long id)
        {
            try
            {
                // Gọi service để lấy thông tin người dùng theo email
                var user = await _userService.GetUserByIDAsync(id);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                return Ok(user); // Trả về thông tin người dùng nếu tìm thấy
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-profile/{Id}")]
        public async Task<IActionResult> UpdateProfile([FromBody] DTO.Request.UserUpdateRequest userUpdate, long Id )
        {
            try
            {
                await _userService.UpdateProfile(userUpdate, Id);
                return Ok("Profile updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}


        
