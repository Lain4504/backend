using System.ComponentModel.DataAnnotations;

namespace BackEnd.DTO.Request
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
