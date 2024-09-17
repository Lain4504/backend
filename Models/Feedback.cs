using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class Feedback
{
    public long Id { get; set; }

    public string? Comment { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? State { get; set; }

    public long? BookId { get; set; }

    public long? UserId { get; set; }

    public virtual Book? Book { get; set; }

    public virtual User? User { get; set; }
}
