using BackEnd.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using BackEnd.DTO.Request;
using Microsoft.AspNetCore.Authorization;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("/api/author")]
    [EnableCors("AllowSpecificOrigins")]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        
        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        // GET: api/Author
        [Authorize(Policy = "AdminRole")]
        [HttpGet]
        public async Task<IActionResult> GetAllAuthors()
        {
            var authors = await _authorService.GetAllAuthors();
            if (authors == null || !authors.Any())
            {
                return NotFound("No authors found.");
            }
            return Ok(authors);
        }
        [Authorize(Policy = "AdminRole")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthorById(long id)
        {
            var author = await _authorService.GetAuthorById(id);
            if (author == null)
            {
                return NotFound("Author not found.");
            }
            return Ok(author);
        }
        [Authorize(Policy = "AdminRole")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(long id)
        {
           await _authorService.DeleteAuthor(id);
           return NoContent();
        }
        [Authorize(Policy = "AdminRole")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAuthor(long id, [FromBody] Author author)
        {
            if (id != author.Id) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _authorService.UpdateAuthor(author);
            return Ok(new { message = "Update successful!" });
        }
        [Authorize(Policy = "AdminRole")]
        [HttpPost]
        public async Task<IActionResult> AddAuthor([FromBody] Author author)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _authorService.AddAuthor(author);
            return CreatedAtAction(nameof(GetAuthorById), new { id = author.Id }, author);
        }
        [Authorize(Policy = "AdminRole")]
        [HttpPost("add-book-to-author")]
        public async Task<IActionResult> AddBookToAuthor([FromBody] AddBookToAuthorRequest request)
        {
            if (request == null || request.BookId <= 0 || request.AuthorId <= 0)
            {
                return BadRequest("Invalid request.");
            }

            var result = await _authorService.AddBookToAuthor(request.BookId, request.AuthorId);

            if (result)
            {
                return Ok("Book successfully added to the author.");
            }

            return BadRequest("Failed to add book to author. Please ensure the book and author exist.");
        }
        [Authorize(Policy = "AdminRole")]
        [HttpDelete("{bookId}/author/{authorId}")]
        public async Task<IActionResult> RemoveAuthorFromBook(long bookId, long authorId)
        {
            // Gọi hàm từ service để xóa Author khỏi Book
            var result = await _authorService.RemoveAuthorFromBook(bookId, authorId);

            // Kiểm tra kết quả và trả về phản hồi phù hợp
            if (result)
            {
                return Ok(new { message = "Author removed from book successfully." });
            }
            else
            {
                return NotFound(new { message = "Author or Book not found." });
            }
        }
    }
}
