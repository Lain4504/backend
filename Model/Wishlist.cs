using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.Model
{
    [Table("Wishlist")]
    public class Wishlist
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [ForeignKey("Book")]
        public long BookId { get; set; }
        public virtual Book Book { get; set; }

        [ForeignKey("User")]
        public long UserId { get; set; }
        public virtual User User { get; set; }

        // Add unique constraint at the model level
        [Index(nameof(BookId), nameof(UserId), IsUnique = true)]
        public class Index
        {
        }
    }
}
