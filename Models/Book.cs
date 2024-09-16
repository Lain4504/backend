using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class Book
{
    public long Id { get; set; }

    public string? Isbn { get; set; }

    public string? Cover { get; set; }

    public string? Description { get; set; }

    public float? Discount { get; set; }

    public int? Page { get; set; }

    public long? Price { get; set; }

    public DateOnly? PublicationDate { get; set; }

    public string? Size { get; set; }

    public int? Sold { get; set; }

    public string? State { get; set; }

    public int? Stock { get; set; }

    public string? Title { get; set; }

    public int? Weight { get; set; }

    public long? CategoryId { get; set; }

    public long? PublisherId { get; set; }

    public virtual BookCategory? Category { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Publisher? Publisher { get; set; }

    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
}
