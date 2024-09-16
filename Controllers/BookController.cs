using BackEnd.Model;
using BackEnd.Service;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("/api/book")]
    [ApiController]
    [EnableCors("AllowSpecificOrigins")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // GET: api/book
        [HttpGet]
        public async Task<IActionResult> GetAllBooks([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            // Implement pagination logic if needed
            var books = await _bookService.GetAllBooksAsync();
            return Ok(books);
        }

        // GET: api/book/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(long id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound("Book not found.");
            }
            return Ok(book);
        }

        // POST: api/book
        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] Book book)
        {
            if (book == null)
            {
                return BadRequest("Invalid book data.");
            }

            try
            {
                var createdBook = await _bookService.SaveBookAsync(book);
                return CreatedAtAction(nameof(GetBookById), new { id = createdBook.Id }, createdBook);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/book/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(long id, [FromBody] Book book)
        {
            if (id != book.Id)
            {
                return BadRequest("ID mismatch.");
            }

            try
            {
                var existingBook = await _bookService.GetBookByIdAsync(id);
                if (existingBook == null)
                {
                    return NotFound("Book not found.");
                }

                await _bookService.UpdateBookAsync(book);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/book/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(long id)
        {
            try
            {
                await _bookService.DeleteBookAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // GET: api/book/search
        [HttpGet("search")]
        public async Task<IActionResult> SearchBooks([FromQuery] string title, [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice, [FromQuery] long? collectionId, [FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var books = await _bookService.GetBooksByNameAsync(title);
            if (minPrice.HasValue && maxPrice.HasValue)
            {
                books = books.Where(b => b.Price >= minPrice.Value && b.Price <= maxPrice.Value);
            }
            // Apply additional filters for collection if needed

            return Ok(books);
        }

        // POST: api/book/{bookId}/collection/{collectionId}
        [HttpPost("{bookId}/collection/{collectionId}")]
        public async Task<IActionResult> AddBookToCollection(long bookId, int collectionId)
        {
            try
            {
                await _bookService.AddBookToCollectionAsync(bookId, collectionId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
