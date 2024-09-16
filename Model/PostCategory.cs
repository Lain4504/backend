namespace BackEnd.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace OnlineBookShop.Models
    {
        [Table("PostCategory")]
        public class PostCategory
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public long Id { get; set; }

            [Required(ErrorMessage = "The category name is required")]
            public string Name { get; set; }

            // Initialize the collection to avoid null reference issues
            public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();
        }
    }

    }
