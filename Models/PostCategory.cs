using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class PostCategory
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
