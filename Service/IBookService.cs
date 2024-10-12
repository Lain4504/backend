using BackEnd.Models;
using BackEnd.Util;

namespace BackEnd.Service
{
    public interface IBookService
    {
        Task<Book?> GetBookByIdAsync(long id);
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book> SaveBookAsync(Book book);
        Task UpdateBookAsync(Book book);
        Task DeleteBookAsync(long id);
        Task<IEnumerable<Book>> FindBooksByTitleAsync(string title);
        Task<bool> AddBookToCollectionAsync(long bookId, long collectionId);
        Task<PaginatedList<Book>> GetAllBooksAsync(int page, int size, string sortBy, bool isAscending);
        Task<IEnumerable<Book>> GetBooksByCollectionAsync(int? collectionId, string sortBy, string sortOrder);
        IEnumerable<BookCollection> GetAllBookCollectionsByBookId(long bookId);
        Task<List<Book>> GetBooksByAuthorIdAsync(long authorId);

    }
}
