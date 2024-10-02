using BackEnd.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("api/author")]
    [EnableCors("AllowSpecificOrigins")]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        // GET: api/Author
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

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteAuthor(long id)
        //{
        //    await _authorService.DeleteAuthor(id);
        //    return NoContent();
        //}
    }
}
