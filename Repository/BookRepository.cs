//using BackEnd.Model;
//using Microsoft.EntityFrameworkCore;

//namespace BackEnd.Repository
//{
//    public class BookRepository : IBookRepository
//    {
//        private readonly BookStoreContext _context;

//        public BookRepository(BookStoreContext context)
//        {
//            _context = context;
//        }

//        public async Task<Book> GetBookByIdAsync(long id)
//        {
//            return await _context.Set<Book>().FindAsync(id);
//        }

//        public async Task<IEnumerable<Book>> GetAllBooksAsync()
//        {
//            return await _context.Set<Book>().ToListAsync();
//        }

//        public async Task<Book> SaveBookAsync(Book book)
//        {
//            await _context.Set<Book>().AddAsync(book);
//            await _context.SaveChangesAsync();
//            return book;
//        }

//        public async Task UpdateBookAsync(Book book)
//        {
//            _context.Set<Book>().Update(book);
//            await _context.SaveChangesAsync();
//        }

//        public async Task DeleteBookAsync(long id)
//        {
//            var book = await GetBookByIdAsync(id);
//            if (book != null)
//            {
//                _context.Set<Book>().Remove(book);
//                await _context.SaveChangesAsync();
//            }
//        }

//        public async Task<bool> BookExistsByISBNAsync(string isbn)
//        {
//            return await _context.Set<Book>().AnyAsync(b => b.ISBN == isbn);
//        }

//        public async Task<IEnumerable<Book>> GetBooksByNameAsync(string name)
//        {
//            return await _context.Set<Book>()
//                .Where(b => b.Title.Contains(name))
//                .ToListAsync();
//        }
//    }
//}
