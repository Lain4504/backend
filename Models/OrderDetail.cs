using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class OrderDetail
{
    public long Id { get; set; }

    public int? Amount { get; set; }

    public long? OriginalPrice { get; set; }

    public long? SalePrice { get; set; }

    public long? BookId { get; set; }

    public long? OrderId { get; set; }

    public virtual Book? Book { get; set; }

    public virtual Order? Order { get; set; }
}
