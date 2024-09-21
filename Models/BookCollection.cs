using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class BookCollection
{

    public long BookId { get; set; }

    public long CollectionId { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Collection Collection { get; set; } = null!;
}
