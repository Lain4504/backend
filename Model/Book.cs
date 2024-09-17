
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Model
{
    [Table("Book")]
    public class Book
    {
        [Key]
        public long Id { get; set; }
        public string Title { get; set; }

        public string Publisher { get; set; }

        public string Authors { get; set; }

        public ICollection<Collection> Collections { get; set; } = new HashSet<Collection>();

        public string Description { get; set; }

        public int Stock { get; set; }

        public int Sold { get; set; }

        public DateTime? PublicationDate { get; set; }

        public string Size { get; set; }

        public int Weight { get; set; }

        public long Price { get; set; }

        public int Page { get; set; }

        public string Cover { get; set; }

        public string ISBN { get; set; }

        public float Discount { get; set; }

        public string category { get; set; }
        public ICollection<Image> Images { get; set; }

        public BookState State { get; set; }
        
    }

    public enum BookState
    {
        Active,
        Inactive,
    }
}
