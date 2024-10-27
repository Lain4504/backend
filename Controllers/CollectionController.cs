using BackEnd.Models;
using BackEnd.Service;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("/api/collection")]
    [ApiController]
    [EnableCors("AllowSpecificOrigins")]
    public class CollectionController : ControllerBase
    {
        private readonly ICollectionService _collectionService;
        public CollectionController(ICollectionService collectionService)
        {
            _collectionService = collectionService;
        }
        [HttpGet]
        public async Task<ActionResult> GetBookCollections()
        {
            try
            {
                var collections = await _collectionService.GetAllCollectionsAsync();
                if (collections == null || collections.Count() == 0)
                {
                    return NoContent();
                }
                return Ok(collections);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error. Please try again later.");

            }

        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetCollectionById(long id)
        {
            try
            {
                var collection = await _collectionService.GetCollectionByIdAsync(id);
                if (collection == null)
                {
                    return NotFound();
                }
                return Ok(collection);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error. Please try again later.");
            }


        }
        [HttpPost("create")]
        public async Task<ActionResult> SaveCollection([FromBody] Collection collection)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _collectionService.SaveCollectionAsync(collection); ;
            return CreatedAtAction(nameof(GetCollectionById), new { id = collection.Id }, collection);
        }
        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateCollection(long id, [FromBody] Collection collection)
        {
            if (id != collection.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _collectionService.UpdateCollectionAsync(collection);
            return Ok(new { message = "Update successful!" });
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCollection(long id)
        {
            var collection = await _collectionService.GetCollectionByIdAsync(id);
            if (collection == null)
            {
                return NotFound();
            }

            await _collectionService.DeleteCollectionAsync(id);
            return NoContent();
        }

        [HttpGet("sorted-and-paged")]
        public async Task<IActionResult> GetAllBookCollections(
       [FromQuery] string sortBy = "Id",
       [FromQuery] int page = 0,
       [FromQuery] int size = 5,
       [FromQuery] string sortOrder = "asc")
        {
            bool isAscending = sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase);
            var bookCollections = await _collectionService.GetAllBookCollectionsAsync(page, size, sortBy, isAscending);
            return Ok(bookCollections);
        }
        [HttpDelete("{bookId}/collection/{collectionId}")]
        public async Task<IActionResult> RemoveCollectionFromBook(long bookId, long collectionId)
        {
            var result = await _collectionService.RemoveCollectionFromBook(bookId, collectionId);

            if (result)
            {
                return Ok(new { message = "Collection removed from book successfully." });
            }
            else
            {
                return NotFound(new { message = "Collection or Book not found." });
            }
        }

    }

}
