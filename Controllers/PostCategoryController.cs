using BackEnd.Models;
using BackEnd.Service;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("/api/post-category")]
    [ApiController]
    [EnableCors("AllowSpecificOrigins")]
    public class PostCategoryController : ControllerBase 
    {
        private readonly IPostCategoryService _PostCategoryService;
        public PostCategoryController(IPostCategoryService PostCategoryService)
        {
            _PostCategoryService = PostCategoryService;
        }

        [HttpPost]
        public async Task<IActionResult> AddPostCategory([FromBody] PostCategory postCategory)
        {
            if (!ModelState.IsValid) BadRequest(ModelState); 
            await _PostCategoryService.AddPostCategoryAsync(postCategory); 
            return CreatedAtAction(nameof(GetPostCategoryById), new { id = postCategory.Id }, postCategory);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult> GetPostCategoryById(long id)
        {
            try
            {
                var postCategories = await _PostCategoryService.GetPostCategoryByIdAsync(id);
                if (postCategories == null) return NotFound();
                return Ok(postCategories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error. Please try again later.");

            }
        }
        
        [HttpGet]
         public async Task<IActionResult> GetAllPostCategories()
        {
            try
            {
                var postCategories = await _PostCategoryService.GetAllPostCategoriesAsync();
                if (postCategories == null)
                    return NotFound();
                return Ok(postCategories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error. Please try again later.");

            }
        }

        [HttpDelete("{id}")]    
        public async Task<IActionResult> DeletePostCategory(long id)
        {
            await _PostCategoryService.DeletePostCategoryAsync(id);
            return NoContent();
        }
       

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePostCategory(long id, [FromBody] PostCategory postCategory)
        {
            if (id != postCategory.Id)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _PostCategoryService.UpdatePostCategoryAsync(postCategory);
            return Ok(new { message = "Update successful!" });
        }
    }
}