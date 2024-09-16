using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BackEnd.Model.OnlineBookShop.Models;

namespace BackEnd.Model
{
    [Table("Post")]
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required(ErrorMessage = "The title of the post is required")]
        public string Title { get; set; }

        [ForeignKey(nameof(Category))]
        public long? CategoryId { get; set; }
        public virtual PostCategory Category { get; set; }

        [ForeignKey(nameof(User))]
        public long? UserId { get; set; }
        public virtual User User { get; set; }

        [Required(ErrorMessage = "The thumbnail of the post is required")]
        public string Thumbnail { get; set; }

        [Required(ErrorMessage = "The content of the post is required")]
        public string Content { get; set; }

        [Required(ErrorMessage = "The brief of the post is required")]
        public string Brief { get; set; }

        public DateTime CreatedAt { get; set; }

        [Required]
        public PostState State { get; set; }
    }

    public enum PostState
    {
        Draft,
        Published,
        Archived
    }
}
