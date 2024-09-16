using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class Slider
{
    public long Id { get; set; }

    public string? BackLink { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public string? Title { get; set; }
}
