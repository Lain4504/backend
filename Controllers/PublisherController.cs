using System.Collections.Generic;
using System.Threading.Tasks;
using BackEnd.Models;
using BackEnd.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowSpecificOrigins")]
    public class PublisherController : ControllerBase
    {
        private readonly IPublisherService _publisherService;

        public PublisherController(IPublisherService publisherService)
        {
            _publisherService = publisherService;
        }
        [Authorize(Policy = "AdminRole")]

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
        [Authorize(Policy = "AdminRole")]

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
        [Authorize(Policy = "AdminRole")]

        // PUT: api/Publisher/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePublisher(long id, [FromBody] Publisher publisher)
        {
            if (id != publisher.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _publisherService.UpdatePublisherAsync(publisher);
            return Ok(new { message = "Update successful!" });
        }
        [Authorize(Policy = "AdminRole")]

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
