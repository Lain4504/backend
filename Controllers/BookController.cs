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
        private readonly BookStoreContext _context;

        public BookController(IBookService bookService, BookStoreContext context)
        {
            _bookService = bookService;
            _context = context;
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
        public async Task<IActionResult> AddBook([FromBody] Book book)
        {
            await _bookService.AddBookAsync(book);
            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
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
                content = paginatedBooks, // Hoặc tùy chỉnh tùy theo cấu trúc của PaginatedList<Book>
                totalPages = paginatedBooks.TotalPages,
                pageIndex = page
            };

            return Ok(response);
        }
        [HttpGet("sorted-and-paged/by-collection")]
        public async Task<ActionResult<PaginatedList<Book>>> GetBooksByCollectionAndPriceBetween(
            [FromQuery] int? collection,
            [FromQuery] int min = 0,
            [FromQuery] int max = int.MaxValue,
            [FromQuery] string sortBy = "Id",
            [FromQuery] int page = 0,
            [FromQuery] int size = 5,
            [FromQuery] string sortOrder = "asc")
        {
            // Tạo truy vấn ban đầu, lọc sách theo khoảng giá
            var query = _context.Books
                .Join(
                    _context.BookCollections,
                    book => book.Id,
                    bookCollection => bookCollection.BookId,
                    (book, bookCollection) => new { Book = book, BookCollection = bookCollection }
                )
                .Where(bc => bc.Book.Price >= min && bc.Book.Price <= max);

            // Nếu có collectionId, lọc sách theo bộ sưu tập
            if (collection.HasValue)
            {
                query = query.Where(bc => bc.BookCollection.CollectionId == collection.Value);
            }

            // Sắp xếp theo trường dữ liệu sortBy và hướng sortOrder
            if (sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase))
            {
                query = query.OrderBy(bc => EF.Property<object>(bc.Book, sortBy));
            }
            else
            {
                query = query.OrderByDescending(bc => EF.Property<object>(bc.Book, sortBy));
            }

            // Loại bỏ các bản ghi sách bị trùng lặp
            var booksQuery = query.Select(bc => bc.Book).Distinct().Include(book => book.Images);

            // Phân trang
            var paginatedBooks =  await PaginatedList<Book>.CreateAsync(booksQuery.AsNoTracking(), page, size);

            var response = new
            {
                content = paginatedBooks, // Hoặc tùy chỉnh tùy theo cấu trúc của PaginatedList<Book>
                totalPages = paginatedBooks.TotalPages,
                pageIndex = page
            };

            return Ok(response);
        }


    }
}
