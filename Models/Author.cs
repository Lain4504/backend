using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class Author
{
    public long Id { get; set; }

    public string? Company { get; set; }

    public string? Name { get; set; }
}
