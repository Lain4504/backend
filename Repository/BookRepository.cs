using BackEnd.Exceptions;
using BackEnd.Model;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly BookStoreContext _context;
        public BookRepository(BookStoreContext context)
        {
            _context = context;
        }
        public async Task<Book> AddBookToCollectionAsync(long bookId, long collectionId)
        {
            var existingBook = await _context.Books.FindAsync(bookId);
            var existingCollection = await _context.Collections.FindAsync(collectionId);
            if (existingBook != null || existingCollection == null)
            {
                throw new MessageException("Resource Not Found");
            }
            if (existingBook.Collections.Contains(existingCollection))
            {
                throw new MessageException("Book Duplicated");
            }
            existingBook.Collections.Add(existingCollection);
            await _context.SaveChangesAsync();
            return existingBook;
        }

        public async Task ChangeBookStateAsync(long id)
        {
            var existingBook = await _context.Books.FindAsync(id);
            if (existingBook == null)
            {
                throw new MessageException("Book is not found");
            }
            existingBook.State = existingBook.State == BookState.Active ? BookState.Active : BookState.Inactive;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBookAsync(long id)
        {
            var existingBook = await _context.Books.FindAsync(id);
            if (existingBook == null)
            {
                throw new MessageException("Book Not Found");
            }
            existingBook.State = BookState.Inactive;
            await _context.SaveChangesAsync();
        }

        public async Task<List<Book>> GetAllBooksAsync()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Book> GetBookByIdAsync(long id)
        {
            var result = await _context.Books.FindAsync(id);
            if (result != null)
            {
                return result;
            }
            return null;
        }

        public async Task<List<Book>> GetBooksByCategoriesAndPriceRangeAsync(List<BookCategory> categories, int min, int max)
        {
            return await _context.Books
                .Where(book => categories.Contains(book.Category) && book.State == BookState.Active && book.Price >= min && book.Price <= max)
                .ToListAsync();
        }

        public async Task<List<Book>> GetBooksByCollectionAndPriceRangesAsync(Collection collection, int min, int max)
        {
            if (collection != null)
            {
                return await _context.Books
                    .Where(book => book.Collections.Contains(collection) && book.State == BookState.Active && book.Price >= min && book.Price <= max)
                    .ToListAsync();
            }
            else
            { 
                return await _context.Books
                    .Where(book => book.State == BookState.Active && book.Price >= min && book.Price <= max)
                    .ToListAsync();
            }
        }

        public async Task<List<Book>> GetBooksByNameAsync(string name)
        {
            return await _context.Books
                .Where(book => book.Title.Contains(name) && book.State == BookState.Active)
                .ToListAsync();
        }

        public async Task<List<Book>> QueryBookAsync(string title, BookState state, Collection collection)
        {
            return await _context.Books
                .Where(book => book.Title.Contains(title) && book.State == state && book.Collections.Contains(collection))
                .ToListAsync();
        }

        public async Task<Book> SaveBookAsync(Book book)
        {
            var existingBook = await _context.Books.FirstOrDefaultAsync(book => book.ISBN == book.ISBN);
            if (existingBook != null)
            {
                throw new MessageException("ISBN is already existed");
            }
            book.State = BookState.Active;
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Book> UpdateBookAsync(Book book)
        {
            var existingBook = await _context.Books.FindAsync(book.Id);
            if (existingBook == null)
            {
                throw new MessageException("Book not found");
            }

            existingBook.Update(book);
            await _context.SaveChangesAsync();
            return existingBook;
        }
    }
}
