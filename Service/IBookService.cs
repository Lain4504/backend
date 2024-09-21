using BackEnd.Models;
using BackEnd.Util;

namespace BackEnd.Service
{
    public interface IBookService
    {
        Task<Book?> GetBookByIdAsync(long id);
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task AddBookAsync(Book book);
        Task UpdateBookAsync(Book book);
        Task DeleteBookAsync(long id);
        Task<IEnumerable<Book>> FindBooksByTitleAsync(string title);
        Task AddBookToCollectionAsync(long bookId, long collectionId);
        Task<PaginatedList<Book>> GetAllBooksAsync(int page, int size, string sortBy, bool isAscending);
    }
}
