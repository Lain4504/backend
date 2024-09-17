using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class Order
{
    public long Id { get; set; }

    public string? Address { get; set; }

    public DateTime? Created { get; set; }

    public string? CustomerNote { get; set; }

    public string? District { get; set; }

    public string? Email { get; set; }

    public string? FullName { get; set; }

    public string? PaymentState { get; set; }

    public string? Phone { get; set; }

    public string? Province { get; set; }

    public long? ShippingPrice { get; set; }

    public string? ShippingState { get; set; }

    public string? ShopNote { get; set; }

    public string? State { get; set; }

    public long? TotalPrice { get; set; }

    public string? Ward { get; set; }

    public long? UserId { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual User? User { get; set; }
}
