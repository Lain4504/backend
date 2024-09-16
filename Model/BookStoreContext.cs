using BackEnd.Model.OnlineBookShop.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Model
{
    public class BookStoreContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<Author> Authors { get; set; }

        public DbSet<BookCategory> BookCategories { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostCategory> PostCategories { get; set; }
        public DbSet<Publisher> Publisher { get; set; }
        public DbSet<Slider> Slider { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public BookStoreContext(DbContextOptions<BookStoreContext> options) : base(options) 
        {
        }
    }
}
