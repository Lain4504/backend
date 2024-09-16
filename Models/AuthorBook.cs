using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class AuthorBook
{
    public long BookId { get; set; }

    public long AuthorId { get; set; }

    public virtual Author Author { get; set; } = null!;

    public virtual Book Book { get; set; } = null!;
}
