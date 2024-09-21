using BackEnd.Models;
using BackEnd.Util;
using System.Linq.Expressions;

namespace BackEnd.Repository
{
    public interface IBookRepository
    {
        Task<Book?> GetByIdAsync(long id);
        Task<IEnumerable<Book>> GetAllAsync();
        Task AddAsync(Book book);
        Task UpdateAsync(Book book);
        Task DeleteAsync(long id);
        Task<bool> ExistsByIsbnAsync(string isbn);
        Task<IEnumerable<Book>> FindByTitleAsync(string title);
        Task AddBookToCollectionAsync(long bookId, long collectionId);
        Task<PaginatedList<Book>> GetAllBooksAsync(int pageIndex, int pageSize, string sortBy, bool isAscending);

    }
}
