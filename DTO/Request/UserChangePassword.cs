using System.ComponentModel.DataAnnotations;

namespace BackEnd.DTO.Request
{
    public class UserChangePassword
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string Token { get; set; }
    }
}
