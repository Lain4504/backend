using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class Image
{
    public long Id { get; set; }

    public string? Description { get; set; }

    public string? Link { get; set; }

    public long? BookId { get; set; }

    public virtual Book? Book { get; set; }
}
