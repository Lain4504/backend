
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Model
{
    [Table("Book")]
    public class Book
    {
        [Key]
        public long Id { get; set; }

        [Required(ErrorMessage = "The title of the book is required")]
        public string Title { get; set; }

        // Replace Publisher, Authors, Language with string
        public string Publisher { get; set; }

        public string Authors { get; set; }

        public string Language { get; set; }

        // Many-to-many with BookCollection
        public ICollection<Collection> Collections { get; set; }

        [Column(TypeName = "text")]
        public string Description { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock must be at least 0")]
        public int Stock { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Sold must be at least 0")]
        public int Sold { get; set; }

        public DateTime? PublicationDate { get; set; }

        public string Size { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The weight must be at least 1")]
        public int Weight { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "The price must be at least 1")]
        public long Price { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The page count must be at least 1")]
        public int Page { get; set; }

        public string Cover { get; set; }

        [Required(ErrorMessage = "The ISBN is required")]
        [StringLength(13, MinimumLength = 10, ErrorMessage = "ISBN must be between 10 and 13 characters")]
        public string ISBN { get; set; }

        [Range(0, 1, ErrorMessage = "The discount must be between 0 and 1")]
        public float Discount { get; set; }

        // Foreign key to BookCategory
        [ForeignKey("CategoryId")]
        public BookCategory Category { get; set; }

        public long? CategoryId { get; set; }

        // One-to-many with Image
        public ICollection<Image> Images { get; set; }

        // Enum for BookState
        public BookState State { get; set; }

        // Calculate sale price after discount
        
    }

    public enum BookState
    {
        Active,
        Inactive,
    }
}
