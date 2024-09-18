using BackEnd.Models;

namespace BackEnd.Service
{
    public interface IBookService
    {
        Task<Book?> GetBookByIdAsync(long id);
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task AddBookAsync(Book book);
        Task UpdateBookAsync(Book book);
        Task DeleteBookAsync(long id);
        Task<bool> ExistsByIsbnAsync(string isbn);
        Task<IEnumerable<Book>> FindBooksByTitleAsync(string title);
        Task AddBookToCollectionAsync(long bookId, long collectionId);
        //Task<IEnumerable<Book>> QueryBooksAsync(string? title, string? state, long? categoryId, long? collectionId);
    }
}
