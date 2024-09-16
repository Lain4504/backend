using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class BookCategory
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
