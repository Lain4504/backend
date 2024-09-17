using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.Model
{
    [Table("Author")]
    public class Author
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required(ErrorMessage = "The author name is required")]
        public string Name { get; set; }

        public string Company { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
