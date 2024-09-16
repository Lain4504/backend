using BackEnd.Model.OnlineBookShop.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.Model
{
    [Table("User")]
    public class User : IdentityUser<long>
    {
        [Required]
        [StringLength(255)]
        public string FullName { get; set; }

        [Required]
        [StringLength(255)]
        public string Province { get; set; }

        [Required]
        [StringLength(255)]
        public string District { get; set; }

        [Required]
        [StringLength(255)]
        public string Ward { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public override string Email { get; set; }

        [Required]
        public string Password { get; set; }  // Use IdentityUser's PasswordHash property

        [Required]
        public string Role { get; set; }  // Enum values can be managed differently in .NET

        public string State { get; set; }  // Enum values can be managed differently in .NET

        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Wishlist> Wishlists { get; set; }
    }
}
