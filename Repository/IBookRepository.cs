using BackEnd.Model;

namespace BackEnd.Repository
{
    public interface IBookRepository
    {
        Task<Book> GetBookByIdAsync(long id);
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book> SaveBookAsync(Book book);
        Task UpdateBookAsync(Book book);
        Task DeleteBookAsync(long id);
        Task<bool> BookExistsByISBNAsync(string isbn);
        Task<IEnumerable<Book>> GetBooksByNameAsync(string name);
    }
}
