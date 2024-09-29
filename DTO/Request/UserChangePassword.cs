using System.ComponentModel.DataAnnotations;

namespace BackEnd.DTO.Request
{
    public class UserChangePassword
    {
        public string OldPassword {  get; set; }
        public string NewPassword { get; set; }
        [Required]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
        public string ConfirmNewPassword { get; set; }
    }
}
