using BackEnd.Models;
using BackEnd.Service;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("/api/post")]
    [ApiController]
    [EnableCors("AllowSpecificOrigins")]
    public class PostController : ControllerBase 
    {
        private readonly IPostService _PostService;
        public PostController(IPostService PostService)
        {
            _PostService = PostService;
        }

        [HttpPost]
        public async Task<IActionResult> AddPost([FromBody] Post post)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _PostService.AddPostAsync(post);
            if (result)
                return CreatedAtAction(nameof(GetPostById), new { id = post.Id }, post);
            return Conflict(new { Message = "Mã Title không được trùng." });
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult> GetPostById(long id)
        {
                var Posts = await _PostService.GetPostByIdAsync(id);
                if (Posts == null) return NotFound();
                return Ok(Posts);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPost()
        {
            try
            {
                var posts = await _PostService.GetAllPostAsync();
                if (posts == null)
                    return NotFound();
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error. Please try again later.");

            }
        }
        [HttpGet("sorted-and-paged")]
         public async Task<IActionResult> GetAllPost(
            [FromQuery] string sortBy = "Id",
            [FromQuery] int page = 0,
            [FromQuery] int size = 5,
            [FromQuery] string sortOrder = "asc")
        {
            bool isAscending = sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase);
            var paginatedPosts = await _PostService.GetAllPostAsync(page, size, sortBy, isAscending);
            var response = new
            {
                content = paginatedPosts, 
                totalPages = paginatedPosts.TotalPages,
                pageIndex = page
            };

            return Ok(response);
        }

        [HttpDelete("{id}")]    
        public async Task<IActionResult> DeletePost(long id)
        {
            await _PostService.DeletePostAsync(id);
            return NoContent();
        }
      
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePost(long id, [FromBody] Post post)
        {
            if (id != post.Id) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _PostService.UpdatePostAsync(post);
            return Ok(new { message = "Update successful!" });
        }
        [HttpGet("sorted-and-paged/by-postcategory")]
        public async Task<ActionResult<object>> GetPostsByPostCategory(
        [FromQuery] int? postcategory,
        [FromQuery] string sortBy = "Id",
        [FromQuery] string sortOrder = "asc")
        {
            var items = await _PostService.GetPostsByPostCategoryAsync(postcategory, sortBy, sortOrder);

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