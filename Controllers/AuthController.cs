using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using BackEnd.Service;
using BackEnd.DTO.Request;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity.Data;
using BackEnd.Models;
using BackEnd.Service.ServiceImpl;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace BackEnd.Controllers
{
    [Route("api/user")]
    [ApiController]
    [EnableCors("AllowSpecificOrigins")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IJwtService _jWTService;
        private readonly IRefreshTokenService _refreshTokenService;
        public AuthController(IUserService userService, IEmailService emailService, IJwtService jWTService, IRefreshTokenService refreshTokenService)
        {
            _userService = userService;
            _emailService = emailService;
            _jWTService = jWTService;
            _refreshTokenService = refreshTokenService;
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

            var token = _jWTService.GenerateJwtToken(user.Email, user.Id, user.Role);


            // Gửi email kích hoạt
            await _emailService.SendActivationEmail(user.Email, token);

            return Ok(new { message = "Registration successful. Please check your email to activate your account." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] DTO.Request.LoginRequest loginRequest)
        {
            if (string.IsNullOrEmpty(loginRequest.email) || string.IsNullOrEmpty(loginRequest.password))
            {
                return BadRequest("Email và mật khẩu không được để trống.");
            }

            try
            {
                var existingUser = await _userService.AuthenticateAsync(loginRequest.email, loginRequest.password);
                if (existingUser == null)
                {
                    return Unauthorized("Email hoặc mật khẩu không đúng.");
                }

                // Tạo Access Token
                var accessToken = _jWTService.GenerateJwtToken(existingUser.Email, existingUser.Id, existingUser.Role);

                // Tạo Refresh Token
                var refreshToken = _jWTService.GenerateRefreshToken();

                // Lưu refresh token vào cơ sở dữ liệu và lấy ExpirationDate
                var expirationDate = await _refreshTokenService.GenerateRefreshToken(existingUser, refreshToken);

                // Trả về cả access token, refresh token, thời gian hết hạn
                return Ok(new
                {
                    message = "Đăng nhập thành công.",
                    token = accessToken,
                    refreshToken = refreshToken,
                    expirationDate = expirationDate // Trả về ExpirationDate
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Đã xảy ra lỗi trong quá trình đăng nhập.");
            }
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
            var claimsPrincipal = _jWTService.ValidateJwtToken(request.Token);

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


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] DTO.Request.ForgotPasswordRequest forgotRequest)
        {
            if (string.IsNullOrEmpty(forgotRequest.Email))
            {
                return BadRequest("Email không hợp lệ.");
            }

            // Lấy thông tin người dùng từ cơ sở dữ liệu
            var user = await _userService.GetUserByEmailAsync(forgotRequest.Email);
            if (user == null)
            {
                return NotFound("Không tìm thấy người dùng với email đã cung cấp.");
            }

            // Tạo token JWT với id và role của người dùng
            var token = _jWTService.GenerateJwtToken(user.Email, user.Id, user.Role);

            // Gửi email với liên kết đặt lại mật khẩu chứa token
            await _emailService.SendResetPasswordEmail(forgotRequest.Email, user.Id, user.Role);
            return Ok("Vui lòng kiểm tra email của bạn để đặt lại mật khẩu.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] DTO.Request.ResetPasswordRequest resetRequest)
        {
            // Kiểm tra tính hợp lệ của token
            var claimsPrincipal = _jWTService.ValidateJwtToken(resetRequest.Token);
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

        [HttpGet("get-profile/{Id}")]
        public async Task<IActionResult> GetProfile(long Id)
        {
            try
            {
                // Gọi service để lấy thông tin người dùng theo email
                var user = await _userService.GetUserByIDAsync(Id);
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
        public async Task<IActionResult> UpdateProfile([FromBody] DTO.Request.UserUpdateRequest userUpdate, long Id)
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
        [Authorize(Policy = "AdminRole")]

        [HttpPut("update-user/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateRoleAndStateRequest userUpdate)
        {
            var user = await _userService.GetUserByIDAsync(id);
            if (user == null) return NotFound();
            if (userUpdate.Role == "ADMIN" && userUpdate.State == "INACTIVE")
            {
                return BadRequest("Admin cannot deactivate their own account.");
            }
            user.Role = userUpdate.Role;  // Cần có thuộc tính Role trong UpdateUserRequest
            user.State = userUpdate.State; // Cần có thuộc tính State trong UpdateUserRequest

            await _userService.UpdateUserRoleAndState(user, id);

            return NoContent();
        }
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] DTO.Request.UserChangePassword userChange)
        {
            // Lấy token từ userChange
            var token = userChange.Token;

            // Xác thực token
            var claimsPrincipal = _jWTService.ValidateJwtToken(token);
            if (claimsPrincipal == null)
            {
                return Unauthorized("Token không hợp lệ hoặc đã hết hạn.");
            }

            // Lấy email từ claim
            var emailClaim = claimsPrincipal.FindFirst(ClaimTypes.Email);
            if (emailClaim == null)
            {
                return Unauthorized("Không tìm thấy email trong token.");
            }

            // Lấy giá trị của emailClaim
            var emailValue = emailClaim.Value;
            if (string.IsNullOrEmpty(emailValue))
            {
                return BadRequest("Email không hợp lệ: Giá trị trống.");
            }

            // Gọi UserService để thay đổi mật khẩu
            var result = await _userService.ChangePassword(userChange, emailValue);
            if (!result)
            {
                return BadRequest("Đặt lại mật khẩu không thành công.");
            }

            return Ok("Mật khẩu đã được đặt lại thành công.");
        }
        [Authorize(Policy = "AdminRole")]
        [HttpGet]
        public async Task<IActionResult> GetAllUser()
        {
            try
            {
                var users = await _userService.GetAllUser();
                if (users == null || users.Count() == 0)
                {
                    return NoContent();
                }
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error. Please try again later.");

            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserByIDAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                await _userService.DeleteUserById(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            // Kiểm tra tính hợp lệ của refresh token
            if (string.IsNullOrWhiteSpace(request.refreshToken))
            {
                return BadRequest("Refresh token không được để trống.");
            }

            var token = await _refreshTokenService.GetRefreshToken(request.refreshToken);
            if (token == null || token.ExpirationDate < DateTime.UtcNow)
            {
                return Unauthorized(new { Error = "Refresh token không hợp lệ hoặc đã hết hạn." });
            }

            var user = await _userService.GetUserByIDAsync(token.UserId);
            if (user == null)
            {
                return Unauthorized(new { Error = "Không tìm thấy người dùng." });
            }

            try
            {
                // Tạo mới JWT token và refresh token
                var newJwtToken = _jWTService.GenerateJwtToken(user.Email, user.Id, user.Role);

                // Kiểm tra nếu việc tạo JWT token thất bại
                if (newJwtToken == null)
                {
                    return StatusCode(500, "Có lỗi xảy ra khi tạo JWT token mới.");
                }

                // Trả về JWT và refresh token (nếu cần tạo refresh token mới)
                return Ok(new { token = newJwtToken, refreshToken = request.refreshToken});
            }
            catch (Exception ex)
            {
                // Log lỗi tại đây nếu cần
                return StatusCode(500, "Có lỗi xảy ra khi tạo JWT token");
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                return BadRequest("Refresh token không được để trống.");
            }

            var token = await _refreshTokenService.GetRefreshToken(request.RefreshToken);
            if (token == null)
            {
                return Unauthorized(new { Error = "Refresh token không hợp lệ." });
            }

            await _refreshTokenService.RemoveRefreshToken(request.RefreshToken);


            return Ok(new { Message = "Đăng xuất thành công." });
        }
        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
        {
            try
            {
                // Xác thực token Google
                var payload = await GoogleJsonWebSignature.ValidateAsync(request.token);

                if (payload == null)
                    return Unauthorized(new { Error = "Invalid Google token" });

                // Lấy thông tin người dùng từ Google
                var userId = payload.Subject; // Google user ID
                var email = payload.Email;

                // Kiểm tra người dùng trong cơ sở dữ liệu
                var user = await _userService.GetUserByEmailAsync(email);

                // Nếu người dùng không tồn tại, tạo mới
                if (user == null)
                {
                    user = new User
                    {
                        Email = email,
                    };

                    // Lưu người dùng mới vào cơ sở dữ liệu
                    await _userService.RegisterAsync(user.Email);
                }

                //var existUser = await _userService.GetUserByEmailAsync(user.Email);

                // Tạo JWT token cho người dùng
                var token = _jWTService.GenerateJwtToken(user.Email, user.Id, user.Role);
                var refreshToken = _jWTService.GenerateRefreshToken();

                // Lưu refresh token vào cơ sở dữ liệu và lấy ExpirationDate
                var expirationDate = await _refreshTokenService.GenerateRefreshToken(user, refreshToken);

                // Trả về cả access token, refresh token, thời gian hết hạn
                return Ok(new
                {
                    token = token,
                    refreshToken = refreshToken,
                    expirationDate = expirationDate // Trả về ExpirationDate
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = "An error occurred: " + ex.Message });
            }
        }

    }
}


        
