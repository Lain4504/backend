using BackEnd.Model;
using BackEnd.Service;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace BackEnd.Controllers
{
    [Route("/api/book")]
    [ApiController]
    [EnableCors("AllowSpecificOrigins")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _service;
        public BookController(IBookService service)
        {
            _service = service;
        }
        [HttpGet("sorted-and-paged")]
        public async Task<IActionResult> GetAllBooksSortedAndPaged(
        [FromQuery] string sortBy = "id",
        [FromQuery] int page = 0,
        [FromQuery] int size = 5,
        [FromQuery] string sortOrder = "asc")
        {
            var books = await _service.GetAllBooksAsync();
            var sortedBooks = SortBooks(books, sortBy, sortOrder);
            var pagedBooks = PaginateBooks(sortedBooks, page, size);
            return Ok(pagedBooks);
        }

        private IEnumerable<Book> SortBooks(IEnumerable<Book> books, string sortBy, string sortOrder)
        {
            switch (sortBy.ToLower())
            {
                case "title":
                    books = sortOrder.ToLower() == "asc"
                        ? books.OrderBy(b => b.Title)
                        : books.OrderByDescending(b => b.Title);
                    break;
                case "price":
                    books = sortOrder.ToLower() == "asc"
                        ? books.OrderBy(b => b.Price)
                        : books.OrderByDescending(b => b.Price);
                    break;
                case "id":
                default:
                    books = sortOrder.ToLower() == "asc"
                        ? books.OrderBy(b => b.Id)
                        : books.OrderByDescending(b => b.Id);
                    break;
            }
            return books;
        }

        private IEnumerable<Book> PaginateBooks(IEnumerable<Book> books, int page, int size)
        {
            return books.Skip(page * size).Take(size);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(long id)
        {
            var book = await _service.GetBookByIdAsync(id);
            return Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> SaveBook([FromBody] Book book)
        {
            var savedBook = await _service.SaveBookAsync(book);
            return CreatedAtAction(nameof(GetBookById), new { id = savedBook.Id }, savedBook);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(long id)
        {
            await _service.DeleteBookAsync(id);
            return NoContent();
        }
        [HttpPut]
        public async Task<IActionResult> UpdateBook([FromBody] Book book)
        {
            var updatedBook = await _service.UpdateBookAsync(book);
            return Ok(updatedBook);
        }
        [HttpGet("sorted-and-paged/{categories}/{min}/{max}")]
        public async Task<IActionResult> GetBooksByCategoriesAndPriceRange(
       [FromRoute] List<BookCategory> categories,
       [FromRoute] int min,
       [FromRoute] int max)
        {
            var books = await _service.GetBooksByCategoriesAndPriceRangeAsync(categories, min, max);
            return Ok(books);
        }
        [HttpGet("collection/{collection}/{min}/{max}")]
        public async Task<IActionResult> GetBooksByCollectionAndPriceRanges(
       [FromRoute] Collection collection,
       [FromRoute] int min,
       [FromRoute] int max)
        {
            var books = await _service.GetBooksByCollectionAndPriceRangesAsync(collection, min, max);
            return Ok(books);
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetBooksByName([FromRoute] string name)
        {
            var books = await _service.GetBooksByNameAsync(name);
            return Ok(books);
        }

        [HttpPost("add-to-collection/{bookId}/{collectionId}")]
        public async Task<IActionResult> AddBookToCollection(long bookId, long collectionId)
        {
            var book = await _service.AddBookToCollectionAsync(bookId, collectionId);
            return Ok(book);
        }
        [HttpGet("query")]
        public async Task<IActionResult> QueryBook(
        [FromQuery] string title,
        [FromQuery] BookState state,
        [FromQuery] Collection collection)
        {
            var books = await _service.QueryBookAsync(title, state, collection);
            return Ok(books);
        }

        [HttpPatch("change-state/{id}")]
        public async Task<IActionResult> ChangeBookState(long id)
        {
            await _service.ChangeBookStateAsync(id);
            return NoContent();
        }
    }
}
