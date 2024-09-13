using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

    namespace BackEnd.Model
{
    [Table("Collection")]
    public class Collection
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required(ErrorMessage ="The name of the collection is required")]
        public required string Name { get; set; }
        public string? Type {  get; set; }
        public bool IsDisplay { get; set; }
        public virtual ICollection<Book> Books { get; set; } = new HashSet<Book>();

    }
}
