using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class Post
{
    public long Id { get; set; }

    public string? Brief { get; set; }

    public string? Content { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? State { get; set; }

    public string? Thumbnail { get; set; }

    public string? Title { get; set; }

    public long? CategoryId { get; set; }

    public long? UserId { get; set; }

    public virtual PostCategory? Category { get; set; }

    public virtual User? User { get; set; }
}
