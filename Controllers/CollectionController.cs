using BackEnd.Model;
using BackEnd.Service;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("/api/collection")]
    [ApiController]
    [EnableCors("AllowSpecificOrigins")]
    public class CollectionController : Controller
    {
        private readonly ICollectionService _collectionService;
        public CollectionController(ICollectionService collectionService)
        {
            _collectionService = collectionService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Collection>>> GetBookCollections()
        {
            try
            {
                var collections = await _collectionService.GetAllCollectionsAsync();
                if (collections == null || collections.Count == 0)
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
        public async Task<ActionResult<Collection>> GetCollectionById(int id)
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
        [HttpPost]
        public async Task<ActionResult<Collection>> SaveCollection([FromBody] Collection collection)
        {
            var createCollection = await _collectionService.SaveCollectionAsync(collection); ;
            return CreatedAtAction(nameof(GetCollectionById), new { id = createCollection.Id }, createCollection);
        }
        [HttpPut]
        public async Task<ActionResult<Collection>> UpdateCollection([FromBody] Collection collection)
        {
            var result = await _collectionService.UpdateCollectionAsync(collection);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCollection(int id)
        {
            var result = await _collectionService.DeleteCollectionAsync(id);  
            if (!result)
                return NotFound();
            return NoContent();
        }

    }
}
