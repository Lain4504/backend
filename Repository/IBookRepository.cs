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
        Task AddBookToCollectionAsync(long bookId, long collectionId);
        Task<PaginatedList<Book>> GetAllBooksAsync(int pageIndex, int pageSize, string sortBy, bool isAscending);
        IQueryable<Book> GetBooksByCollection(int collectionId);
        IQueryable<Book> GetBooks();
    }
}
