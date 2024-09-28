using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class User
{
    public long Id { get; set; }

    public string? Email { get; set; }

    public string? FullName { get; set; }

    public string? Password { get; set; }

    public string? Role { get; set; }
  
    public string? State {  get; set; } 

    public string? Phone { get; set; }

    public string? Address {  get; set; }

    public DateTime? Dob { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
}
