using BackEnd.Models;
using BackEnd.Util;
using System.Linq.Expressions;

namespace BackEnd.Repository
{
    public interface IBookRepository
    {
        Task<Book?> GetByIdAsync(long id);
        Task<IEnumerable<Book>> GetAllAsync();
        Task<Book> SaveAsync(Book book);
        Task UpdateAsync(Book book);
        Task DeleteAsync(long id);
        Task<bool> ExistsByISBNAsync(string isbn);
        Task<IEnumerable<Book>> FindByTitleAsync(string title);
        Task<bool> AddBookToCollectionAsync(long bookId, long collectionId);
        IQueryable<Book> GetBooksByCollection(int collectionId);
        IQueryable<Book> GetBooks();
        IEnumerable<BookCollection> GetAllBookCollectionsByBookId(long bookId);
        IEnumerable<AuthorBook> GetAllAuthorsByBookId(long bookId);
        Task<List<Book>> GetBooksByAuthorIdAsync(long authorId);
        Task<IEnumerable<Book>> GetAllBooksAsync(int page, int size, string sortBy, bool isAscending);
        Task<int> GetTotalBooksCountAsync();
    }
}
