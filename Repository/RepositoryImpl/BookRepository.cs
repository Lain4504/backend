using BackEnd.Models;
using BackEnd.Util;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace BackEnd.Repository.RepositoryImpl
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
        public async Task<IEnumerable<Book>> FindByTitleAsync(string title)
        {
            return await _context.Books
                .Where(b => b.Title.Contains(title))
                .Include(b => b.Images)
                .ToListAsync();
        }

        public async Task<Book> SaveAsync(Book book)
        {
            if (!string.IsNullOrEmpty(book.Title))
            {
                book.Title = book.Title.Trim();
                while (book.Title.Contains("  "))
                    book.Title = book.Title.Replace("  ", " ");
            }

            // Check for existing book by title
            var existingBook = await _context.Books
                .FirstOrDefaultAsync(b => b.Title == book.Title);

            if (existingBook == null)
            {
                book.PublicationDate = DateOnly.FromDateTime(DateTime.UtcNow); // Set PublicationDate to today
                _context.Books.Add(book);
                await _context.SaveChangesAsync();
                return book;
            }

            return null; // Return null if book already exists
        }

        public async Task UpdateAsync(Book book)
        {
             if (!string.IsNullOrEmpty(book.Title))
                 {
                      book.Title = book.Title.Trim();

                         while (book.Title.Contains("  "))
                          book.Title = book.Title.Replace("  ", " ");
                 }
              _context.Books.Update(book);
           await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long bookId)
        {
            // Find the book by ID
            var book = await _context.Books.Include(b => b.Images)
                                             .FirstOrDefaultAsync(b => b.Id == bookId);
            if (book != null)
            {
                // Delete related records in author_book
                var authorBooks = await _context.AuthorBooks
                    .Where(ab => ab.BookId == bookId)
                    .ToListAsync();
                _context.AuthorBooks.RemoveRange(authorBooks);

                // Delete related records in book_collection
                var collectionsToDelete = await _context.BookCollections
                    .Where(bc => bc.BookId == bookId)
                    .ToListAsync();
                _context.BookCollections.RemoveRange(collectionsToDelete);
                // Delete related images
                var imagesToDelete = await _context.Images
                    .Where(i => i.BookId == bookId)
                    .ToListAsync();
                _context.Images.RemoveRange(imagesToDelete);
                var commentToDelete = await _context.Feedbacks
                    .Where(c => c.BookId == bookId)
                    .ToListAsync();
                // Delete the book
                _context.Books.Remove(book);

                // Save changes
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> ExistsByISBNAsync(string isbn)
        {
            return await _context.Books.AnyAsync(b => b.Isbn == isbn);
        }

        public async Task<IEnumerable<Book>> FindByConditionAsync(Expression<Func<Book, bool>> predicate)
        {
            return await _context.Books.Where(predicate).ToListAsync();
        }

        public async Task<bool> AddBookToCollectionAsync(long bookId, long collectionId)
        {
            var book = await _context.Books.FindAsync(bookId);
            var collection = await _context.Collections.FindAsync(collectionId);

            if (book == null)
            {
                throw new ArgumentException($"Book with ID {bookId} does not exist.");
            }

            if (collection == null)
            {
                throw new ArgumentException($"Collection with ID {collectionId} does not exist.");
            }

            // Check if the book is already in the specified collection
            var exists = await _context.BookCollections
                .AnyAsync(bc => bc.BookId == bookId && bc.CollectionId == collectionId);

            if (exists)
            {
                return false; // The book is already in the collection, so do not add it
            }

            // If the book is not in the collection, add it
            var bookCollection = new BookCollection
            {
                BookId = bookId,
                CollectionId = collectionId
            };

            _context.BookCollections.Add(bookCollection);
            await _context.SaveChangesAsync();
            return true; // Successfully added
        }

        public async Task<PaginatedList<Book>> GetAllBooksAsync(int page, int size, string sortBy, bool isAscending)
        {
            var source = _context.Books
                .Include(b => b.Images)
                .AsQueryable();

            if (isAscending)
            {
                source = source.OrderBy(book => EF.Property<object>(book, sortBy));
            }
            else
            {
                source = source.OrderByDescending(book => EF.Property<object>(book, sortBy));
            }

            return await PaginatedList<Book>.CreateAsync(source, page, size);
        }
        public IQueryable<Book> GetBooksByCollection(int collectionId)
        {
            return from b in _context.Books
                   join bc in _context.BookCollections on b.Id equals bc.BookId
                   where bc.CollectionId == collectionId
                   select b;
        }
        public IQueryable<Book> GetBooks()
        {
            return _context.Books.AsQueryable();
        }
        public IEnumerable<BookCollection> GetAllBookCollectionsByBookId(long bookId)
        {
            return _context.BookCollections
                .Include(bc => bc.Collection) // Include the related Collection
                .Where(bc => bc.BookId == bookId)
                .ToList();
        }
        public IEnumerable<AuthorBook> GetAllAuthorsByBookId(long bookId)
        {
            return _context.AuthorBooks
                .Include(bc => bc.Author) 
                .Where(bc => bc.BookId == bookId)
                .ToList();
        }
        public async Task<List<Book>> GetBooksByAuthorIdAsync(long authorId)
        {
            return await _context.AuthorBooks
                .Where(ab => ab.AuthorId == authorId)
                .Include(ab => ab.Book.Images) 
                .Select(ab => ab.Book) 
                .ToListAsync();
        }
    }

}
