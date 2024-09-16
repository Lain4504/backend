using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class Wishlist
{
    public long Id { get; set; }

    public long? BookId { get; set; }

    public long? UserId { get; set; }

    public virtual Book? Book { get; set; }

    public virtual User? User { get; set; }
}
