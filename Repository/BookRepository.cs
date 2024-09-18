using BackEnd.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace BackEnd.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly BookStoreContext _context;

        public BookRepository(BookStoreContext context)
        {
            _context = context;
        }

        public async Task<Book> GetByIdAsync(long id)
        {
            return await _context.Books
               .Include(b => b.Images)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _context.Books
                .Include(b => b.Images)
                .ToListAsync();
        }

        public async Task AddAsync(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsByIsbnAsync(string isbn)
        {
            return await _context.Books.AnyAsync(b => b.Isbn == isbn);
        }

        public async Task<IEnumerable<Book>> FindByTitleAsync(string title)
        {
            return await _context.Books.Where(b => b.Title.Contains(title)).ToListAsync();
        }

        public async Task<IEnumerable<Book>> FindByConditionAsync(Expression<Func<Book, bool>> predicate)
        {
            return await _context.Books.Where(predicate).ToListAsync();
        }

        public async Task AddBookToCollectionAsync(long bookId, long collectionId)
        {
            var book = await _context.Books.FindAsync(bookId);
            var collection = await _context.Collections.FindAsync(collectionId);

            if (book != null && collection != null)
            {
                var bookCollection = new BookCollection
                {
                    BookId = bookId,
                    CollectionId = collectionId
                };

                _context.BookCollections.Add(bookCollection);
                await _context.SaveChangesAsync();
            }
        }
    }
}
