using BackEnd.Model;

namespace BackEnd.Service
{
    public interface IBookService
    {
        Task<List<Book>> GetAllBooksAsync();
        Task<Book> GetBookByIdAsync(long id);
        Task<Book> SaveBookAsync(Book book);
        Task DeleteBookAsync(long id);
        Task<Book> UpdateBookAsync(Book book);
        Task<List<Book>> GetBooksByCategoriesAndPriceRangeAsync(List<BookCategory> categories, int min, int max);
        Task<List<Book>> GetBooksByCollectionAndPriceRangesAsync(Collection collection, int min, int max);
        Task<List<Book>> GetBooksByNameAsync(string name);
        Task<Book> AddBookToCollectionAsync(long bookId, long collectionId);
        Task<List<Book>> QueryBookAsync(string title, BookState state, Collection collection);
        Task ChangeBookStateAsync(long id);
    }
}
