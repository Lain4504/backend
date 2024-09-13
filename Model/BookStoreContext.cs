using Microsoft.EntityFrameworkCore;

namespace BackEnd.Model
{
    public class BookStoreContext : DbContext
    {
        public DbSet<Collection> Collections { get; set; }
        public DbSet<Book> Books { get; set; }
        public BookStoreContext(DbContextOptions<BookStoreContext> options) : base(options) 
        {
            
        }
      
      
        
    }
}
