using BackEnd.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("api/author")]
    [EnableCors("AllowSpecificOrigins")]

 //Temp code for getAllAuthor
    public class AuthorController : ControllerBase
    {
        private readonly BookStoreContext _context;

        public AuthorController(BookStoreContext context)
        {
            _context = context;
        }

        // GET: api/Author
        [HttpGet]
        public ActionResult<IEnumerable<Author>> GetAllAuthors()
        {
            try
            {
                var authors = _context.Authors.ToList();

                if (authors == null || authors.Count == 0)
                {
                    return NotFound("No authors found.");
                }

                return Ok(authors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}