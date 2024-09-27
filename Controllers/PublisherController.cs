using System.Collections.Generic;
using System.Threading.Tasks;
using BackEnd.Models;
using BackEnd.Service;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublisherController : ControllerBase
    {
        private readonly IPublisherService _publisherService;

        public PublisherController(IPublisherService publisherService)
        {
            _publisherService = publisherService;
        }

        // GET: api/Publisher
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Publisher>>> GetAllPublishers()
        {
            var publishers = await _publisherService.GetAllPublishersAsync();
            return Ok(publishers);
        }

        // GET: api/Publisher/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Publisher>> GetPublisherById(long id)
        {
            var publisher = await _publisherService.GetPublisherByIdAsync(id);
            if (publisher == null)
            {
                return NotFound();
            }
            return Ok(publisher);
        }

        // POST: api/Publisher
        [HttpPost]
        public async Task<ActionResult> CreatePublisher([FromBody] Publisher publisher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _publisherService.CreatePublisherAsync(publisher);
            return CreatedAtAction(nameof(GetPublisherById), new { id = publisher.Id }, publisher);
        }

        // PUT: api/Publisher/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePublisher(long id, [FromBody] Publisher updatedPublisher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var publisher = await _publisherService.GetPublisherByIdAsync(id);
            if (publisher == null)
            {
                return NotFound();
            }

            await _publisherService.UpdatePublisherAsync(id, updatedPublisher.Name);
            return NoContent();
        }

        // DELETE: api/Publisher/{id} 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePublisher(long id)
        {
            var publisher = await _publisherService.GetPublisherByIdAsync(id);
            if (publisher == null)
            {
                return NotFound();
            }

            await _publisherService.DeletePublisherAsync(id);
            return NoContent();
        }
    }
}
