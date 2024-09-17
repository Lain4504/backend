using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.Model
{
    [Table("Image")]
    public class Image
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [ForeignKey("Book")]
        public long BookId { get; set; }
        public virtual Book Book { get; set; }

        [Required(ErrorMessage = "The link is not blank")]
        public string Link { get; set; }

        public string Description { get; set; }
    }
}
