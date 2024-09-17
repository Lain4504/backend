using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.Model
{
    [Table("BookCategory")]
    public class BookCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required(ErrorMessage = "The category name is required")]
        public string Name { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
