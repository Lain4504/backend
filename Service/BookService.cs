using BackEnd.Model;
using BackEnd.Repository;

namespace BackEnd.Service
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICollectionRepository _collectionRepository;
        private readonly IImageRepository _imageRepository;

        public BookService(IBookRepository bookRepository, ICollectionRepository collectionRepository, IImageRepository imageRepository)
        {
            _bookRepository = bookRepository;
            _collectionRepository = collectionRepository;
            _imageRepository = imageRepository;
        }

        public async Task<Book> GetBookByIdAsync(long id)
        {
            return await _bookRepository.GetBookByIdAsync(id);
        }

        public async Task<Book> SaveBookAsync(Book book)
        {
            if (await _bookRepository.BookExistsByISBNAsync(book.ISBN))
            {
                throw new Exception("ISBN already exists.");
            }
            book.State = BookState.Active;
            return await _bookRepository.SaveBookAsync(book);
        }

        public async Task UpdateBookAsync(Book book)
        {
            var existingBook = await _bookRepository.GetBookByIdAsync(book.Id);
            if (existingBook == null)
            {
                throw new Exception("Book not found.");
            }

            existingBook.Title = book.Title;
            existingBook.Description = book.Description;
            existingBook.ISBN = book.ISBN;
            existingBook.Page = book.Page;
            existingBook.Cover = book.Cover;
            existingBook.Price = book.Price;
            existingBook.Discount = book.Discount;
            existingBook.Language = book.Language;
            existingBook.Publisher = book.Publisher;
            existingBook.Stock = book.Stock;
            existingBook.Weight = book.Weight;
            existingBook.Size = book.Size;
            existingBook.State = book.State;
            existingBook.Images = book.Images;
            existingBook.Collections = book.Collections;

            await _imageRepository.DeleteImagesAsync(existingBook.Images);
            foreach (var image in book.Images)
            {
                await _imageRepository.SaveImageAsync(image);
            }

            await _bookRepository.UpdateBookAsync(existingBook);
        }

        public async Task DeleteBookAsync(long id)
        {
            var existingBook = await _bookRepository.GetBookByIdAsync(id);
            if (existingBook == null)
            {
                throw new Exception("Book not found.");
            }
            existingBook.State = BookState.Inactive;
            await _bookRepository.UpdateBookAsync(existingBook);
        }

        //public async Task<IEnumerable<Book>> GetBooksByCategoryAndPriceRangeAsync(IEnumerable<BookCategory> categories, decimal min, decimal max)
        //{
        //    return await _bookRepository.GetBooksByCategoryAndPriceRangeAsync(categories, min, max);
        //}

        //public async Task<IEnumerable<Book>> GetBooksByCollectionAndPriceRangesAsync(BookCollection collection, decimal min, decimal max)
        //{
        //    return await _bookRepository.GetBooksByCollectionAndPriceRangesAsync(collection, min, max);
        //}

        public async Task<IEnumerable<Book>> GetBooksByNameAsync(string name)
        {
            return await _bookRepository.GetBooksByNameAsync(name);
        }

        public async Task AddBookToCollectionAsync(long bookId, int collectionId)
        {
            var existingBook = await _bookRepository.GetBookByIdAsync(bookId);
            var existingCollection = await _collectionRepository.GetCollectionByIdAsync(collectionId);

            if (existingBook == null || existingCollection == null)
            {
                throw new Exception("Book or collection not found.");
            }

            if (existingBook.Collections.Contains(existingCollection))
            {
                throw new Exception("Book already in collection.");
            }

            existingBook.Collections.Add(existingCollection);
            await _bookRepository.UpdateBookAsync(existingBook);
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _bookRepository.GetAllBooksAsync();
        }
    }
}
