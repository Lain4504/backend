using BackEnd.Models;
using BackEnd.Service;
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
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(long id)
        {
            await _bookService.DeleteBookAsync(id);
            return NoContent();
        }

        [HttpPost("add-to-collection")]
        public async Task<IActionResult> AddBookToCollection(long bookId, long collectionId)
        {
            await _bookService.AddBookToCollectionAsync(bookId, collectionId);
            return NoContent();
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
        //    [HttpGet("sorted-and-paged/by-collection")]
        //    public async Task<ActionResult<object>> GetBooksByCollectionAndPriceBetween(
        //     [FromQuery] int? collection,
        //     [FromQuery] int? min = 0,
        //     [FromQuery] int? max = 0,
        //     [FromQuery] string sortBy = "Id",
        //     [FromQuery] int page = 0,
        //     [FromQuery] int size = 5,
        //     [FromQuery] string sortOrder = "asc")
        //    {
        //        if (max == 0) max = int.MaxValue;

        //        var booksQuery = _context.Books.AsQueryable();

        //        if (collection.HasValue)
        //        {
        //            booksQuery = from b in booksQuery
        //                         join bc in _context.BookCollections on b.Id equals bc.BookId
        //                         where bc.CollectionId == collection.Value
        //                         select b;
        //        }

        //        // Thêm điều kiện lọc giá
        //        booksQuery = booksQuery.Where(b => b.Price >= min && b.Price <= max);

        //        // Thêm điều kiện sắp xếp
        //        booksQuery = sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase)
        //            ? booksQuery.OrderBy(b => EF.Property<object>(b, sortBy))
        //            : booksQuery.OrderByDescending(b => EF.Property<object>(b, sortBy));

        //        // Thực hiện phân trang
        //        var totalCount = await booksQuery.CountAsync();
        //        var items = await booksQuery.Skip(page * size).Take(size).Include(b => b.Images).ToListAsync();

        //        var totalPages = (int)Math.Ceiling((double)totalCount / size);

        //        var response = new
        //        {
        //            content = items,
        //            pageable = new
        //            {
        //                sort = new
        //                {
        //                    empty = false,
        //                    sorted = sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase),
        //                    unsorted = !sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase)
        //                },
        //                offset = page * size,
        //                pageSize = size,
        //                pageNumber = page,
        //                paged = true,
        //                unpaged = false
        //            },
        //            totalElements = totalCount,
        //            totalPages = totalPages,
        //            last = page >= totalPages - 1,
        //            size = size,
        //            number = page,
        //            first = page == 0,
        //            numberOfElements = items.Count,
        //            empty = !items.Any()
        //        };

        //        return Ok(response);
        //    }
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
    }

}



