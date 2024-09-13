using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Identity.Client;

namespace BackEnd.Model
{
    [Table("Book")]
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required(ErrorMessage = "The title of the book is required")]
        public required string Title { get; set; }

        //[ForeignKey("PublisherId")]
        //public virtual Publisher Publisher { get; set; }
        //public long? PublisherId { get; set; }


        //[ForeignKey("CategoryId")]
        //public virtual BookCategory Category { get; set; }
        //public long? CategoryId { get; set; }

        // Many-to-many relationship with Author
        //public virtual ICollection<Author> Authors { get; set; } = new HashSet<Author>();

        // Many-to-many relationship with BookCollection
        public virtual ICollection<Collection> Collections { get; set; } = new HashSet<Collection>();
        
        [Column(TypeName = "text")]
        public required string Description { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "stock must be at least 0")]
        public int? Stock { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Sold must be at least 0")]
        public int? Sold { get; set; }

        public DateTime? PublicationDate { get; set; }

        public required string Size { get; set; }


        [Range(1, long.MaxValue, ErrorMessage = "The price must be at least 1")]
        public long? Price { get; set; }

        [Range(0.0, 1.0, ErrorMessage = "The discount must be between 0 and 1")]
        public float? Discount { get; set; }

        public required string ISBN { get; set; }

        public BookState? State { get; set; }

        // One-to-many relationships
        //public virtual ICollection<Feedback> Feedbacks { get; set; } = new HashSet<Feedback>();
        //public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new HashSet<OrderDetail>();
        //public virtual ICollection<Wishlist> Wishlists { get; set; } = new HashSet<Wishlist>();
        //public virtual ICollection<Image> Images { get; set; } = new HashSet<Image>();

        [NotMapped]
        public long? SalePrice
        {
            get
            {
                if (Price.HasValue && Discount.HasValue)
                {
                    return Price.Value - (long)(Price.Value * Discount.Value);
                }
                return null;
            }
        }
    }
    public enum BookState
    {
        Active,
        Inactive,
    }
}
