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

                // Delete the book
                _context.Books.Remove(book);

                // Save changes
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

        public async Task<PaginatedList<Book>> GetAllBooksAsync(int page, int size, string sortBy, bool isAscending)
        {
            // Tải dữ liệu liên quan sử dụng Include và ThenInclude
            var source = _context.Books
                .Include(b => b.Images)
                .AsQueryable();

            // Sắp xếp theo thuộc tính mong muốn
            if (isAscending)
            {
                source = source.OrderBy(book => EF.Property<object>(book, sortBy));
            }
            else
            {
                source = source.OrderByDescending(book => EF.Property<object>(book, sortBy));
            }

            // Trả về danh sách phân trang
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

    }

}
