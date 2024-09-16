using BackEnd.Model;

namespace BackEnd.Service
{
    public interface IBookService
    {
        Task<Book> GetBookByIdAsync(long id);
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book> SaveBookAsync(Book book);
        Task UpdateBookAsync(Book book);
        Task DeleteBookAsync(long id);
        //Task<IEnumerable<Book>> GetBooksByCategoryAndPriceRangeAsync(IEnumerable<BookCategory> categories, decimal min, decimal max);
        //Task<IEnumerable<Book>> GetBooksByCollectionAndPriceRangesAsync(Collection collection, decimal min, decimal max);
        Task<IEnumerable<Book>> GetBooksByNameAsync(string name);
        Task AddBookToCollectionAsync(long bookId, int collectionId);
    }
}
