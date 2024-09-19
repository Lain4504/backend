﻿using BackEnd.Models;
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



    }
}
