using System.ComponentModel.DataAnnotations;

namespace BackEnd.DTO.Request
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password should be at least 6 characters long.")]
        public string password { get; set; }

    }
}
