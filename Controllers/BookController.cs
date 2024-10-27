using BackEnd.DTO.Request;
using BackEnd.Models;
using BackEnd.Service;
using BackEnd.Service.ServiceImpl;
using BackEnd.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(long id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null) return NotFound();
            return Ok(book);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            try
            {
                var books = await _bookService.GetAllBooksAsync();
                if (books == null || books.Count() == 0)
                {
                    return NoContent();
                }
                return Ok(books);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error. Please try again later.");

            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveBook([FromBody] Book book)
        {
            if (book == null)
            {
                return BadRequest(new { Message = "Book data is required." }); // HTTP 400 Bad Request
            }

            var result = await _bookService.SaveBookAsync(book);
            if (result == null)
            {
                return Conflict(new { Message = "Mã ISBN không thể bị trùng." }); // HTTP 409 Conflict
            }

            return CreatedAtAction(nameof(SaveBook), new { id = result.Id }, result); // HTTP 201 Created
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(long id, [FromBody] Book book)
        {
            if (id != book.Id) return BadRequest();
            await _bookService.UpdateBookAsync(book);
            return Ok(book);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(long id)
        {
            await _bookService.DeleteBookAsync(id);
            return NoContent();
        }

        [HttpPost("add-to-collection")]
        public async Task<IActionResult> AddBookToCollection([FromBody] AddBookToCollectionRequest request)
        {
            if (request == null || request.BookId <= 0 || request.CollectionId <= 0)
            {
                return BadRequest("Invalid request.");
            }

            // Logic to add the book to the specified collection
            var result = await _bookService.AddBookToCollectionAsync(request.BookId, request.CollectionId);

            if (result)
            {
                return Ok("Book successfully added to the collection.");
            }

            return BadRequest("Failed to add book to collection. Please ensure the book and collection exist.");
        }

        [HttpGet("sorted-and-paged")]
        public async Task<IActionResult> GetAllBooks(
    [FromQuery] string sortBy = "Id",
    [FromQuery] int page = 0,
    [FromQuery] int size = 5,
    [FromQuery] string sortOrder = "asc")
        {
            bool isAscending = sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase);
            var paginatedBooks = await _bookService.GetAllBooksAsync(page, size, sortBy, isAscending);

            // Trả về dữ liệu với thuộc tính 'content'
            var response = new
            {
                content = paginatedBooks, 
                totalPages = paginatedBooks.TotalPages,
                pageIndex = page
            };

            return Ok(response);
        }
        
        [HttpGet("sorted-and-paged/by-collection")]
        public async Task<ActionResult<object>> GetBooksByCollection(
        [FromQuery] int? collection,
        [FromQuery] string sortBy = "Id",
        [FromQuery] string sortOrder = "asc")
        {
            var items = await _bookService.GetBooksByCollectionAsync(collection, sortBy, sortOrder);

            var totalCount = items.Count();

            var response = new
            {
                content = items,
                totalElements = totalCount,
                empty = !items.Any()
            };

            return Ok(response);
        }
        [HttpGet("search")]
        public async Task<IActionResult> GetBooksByTitle([FromQuery] string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                return BadRequest("Title cannot be emtry");
            }
            var books = await _bookService.FindBooksByTitleAsync(title);
            if (!books.Any())
            {
                return NotFound("No books found with the given title");
            }
            return Ok(new { content = books });
        }
        [HttpGet("get-collections/{bookId}")]
        public IActionResult GetAllBookCollectionsByBookId(long bookId)
        {
            var bookCollections = _bookService.GetAllBookCollectionsByBookId(bookId);
            if(bookCollections == null)
            {
                return NotFound();
            }
            return Ok(bookCollections);
        }
        [HttpGet("get-authors/{bookId}")]
        public IActionResult GetAllAuthorsByBookId(long bookId)
        {
            var bookAuthors = _bookService.GetAllAuthorsByBookId(bookId);
            if (bookAuthors == null || !bookAuthors.Any())
            {
                return NotFound();
            }
            return Ok(bookAuthors);
        }
        [HttpGet("author/{authorId}")]
        public async Task<ActionResult<List<Book>>> GetBooksByAuthorId( long authorId)
        {
            var books = await _bookService.GetBooksByAuthorIdAsync(authorId);
            if (books == null || books.Count == 0)
            {
                return NotFound();
            }
            return Ok(books);
        }
    }

}



