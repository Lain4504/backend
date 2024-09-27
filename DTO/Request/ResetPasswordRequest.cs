using System.ComponentModel.DataAnnotations;

namespace BackEnd.DTO.Request
{
    public class ResetPasswordRequest
    {
        [Required]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Token { get; set; } // Token để xác minh yêu cầu
    }
}
