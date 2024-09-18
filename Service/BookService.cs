using Azure;
using BackEnd.Models;
using BackEnd.Repository;

namespace BackEnd.Service
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<Book?> GetBookByIdAsync(long id)
        {
            return await _bookRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _bookRepository.GetAllAsync();
        }

        public async Task AddBookAsync(Book book)
        {
            await _bookRepository.AddAsync(book);
        }

        public async Task UpdateBookAsync(Book book)
        {
            await _bookRepository.UpdateAsync(book);
        }

        public async Task DeleteBookAsync(long id)
        {
            await _bookRepository.DeleteAsync(id);
        }

        public async Task<bool> ExistsByIsbnAsync(string isbn)
        {
            return await _bookRepository.ExistsByIsbnAsync(isbn);
        }

        public async Task<IEnumerable<Book>> FindBooksByTitleAsync(string title)
        {
            return await _bookRepository.FindByTitleAsync(title);
        }

        public async Task AddBookToCollectionAsync(long bookId, long collectionId)
        {
            await _bookRepository.AddBookToCollectionAsync(bookId, collectionId);
        }

        //public async Task<IEnumerable<Book>> QueryBooksAsync(string? title, string? state, long? categoryId, long? collectionId)
        //{
        //    return await _bookRepository.FindByConditionAsync(b =>
        //        (title == null || b.Title.Contains(title)) &&
        //        (state == null || b.State == state) &&
        //        (categoryId == null || b.CategoryId == categoryId) &&
        //        (collectionId == null)
        //    );
        //}
    }
}
