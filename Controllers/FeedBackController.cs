using BackEnd.Models;
using BackEnd.Service;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("/api/feedback")]
    [ApiController]
    [EnableCors("AllowSpecificOrigins")]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedBackService _feedBackService;
        public FeedbackController(IFeedBackService feedBackService)
        {
            _feedBackService = feedBackService;
        }
        
        [HttpGet("{bookId}")]
        public async Task<ActionResult> GetFeedBackByBookId(long bookId)
        {
            try
            {
                var collection = await _feedBackService.GetFeedBack(bookId);
                if (collection == null)
                {
                    return NotFound();
                }
                return Ok(collection);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error. Please try again later.");
            }

        }

    }
}
