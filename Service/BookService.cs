using BackEnd.Model;
using BackEnd.Repository;

namespace BackEnd.Service
{
    public class BookService : IBookService
    {
        private readonly BookRepository _repository;
        public BookService(BookRepository repository)
        {
            _repository = repository;
        }

        public Task<List<Book>> GetAllBooksAsync() => _repository.GetAllBooksAsync();

        public Task<Book> GetBookByIdAsync(long id) => _repository.GetBookByIdAsync(id);

        public Task<Book> SaveBookAsync(Book book) => _repository.SaveBookAsync(book);

        public Task DeleteBookAsync(long id) => _repository.DeleteBookAsync(id);

        public Task<Book> UpdateBookAsync(Book book) => _repository.UpdateBookAsync(book);

        public Task<List<Book>> GetBooksByCategoriesAndPriceRangeAsync(List<BookCategory> categories, int min, int max)
            => _repository.GetBooksByCategoriesAndPriceRangeAsync(categories, min, max);

        public Task<List<Book>> GetBooksByCollectionAndPriceRangesAsync(Collection collection, int min, int max)
            => _repository.GetBooksByCollectionAndPriceRangesAsync(collection, min, max);

        public Task<List<Book>> GetBooksByNameAsync(string name) => _repository.GetBooksByNameAsync(name);

        public Task<Book> AddBookToCollectionAsync(long bookId, long collectionId)
            => _repository.AddBookToCollectionAsync(bookId, collectionId);

        public Task<List<Book>> QueryBookAsync(string title, BookState state, Collection collection)
            => _repository.QueryBookAsync(title, state, collection);

        public Task ChangeBookStateAsync(long id) => _repository.ChangeBookStateAsync(id);
    }
}
