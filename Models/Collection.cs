using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class Collection
{
    public long Id { get; set; }

    public bool? IsDisplay { get; set; }

    public string? Name { get; set; }

    public string? Type { get; set; }
}
