using BackEnd.Models;
using BackEnd.Service;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BackEnd.Controllers
{
    [Route("/api/feedback")]
    [ApiController]
    [EnableCors("AllowSpecificOrigins")]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedBackService _feedBackService;
        private readonly IHubContext<CommentHub> _commentHub;
        public FeedbackController(IFeedBackService feedBackService, IHubContext<CommentHub> hubContext)
        {
            _feedBackService = feedBackService;
            _commentHub = hubContext;
        }

        [HttpGet("{bookId}")]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetFeedBackByBookId(long bookId)
        {
            try
            {
                var collection = await _feedBackService.GetFeedBack(bookId);
                if (collection == null || !collection.Any())
                {
                    return NotFound("No feedback found for this book.");
                }
                return Ok(collection);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostComment(Feedback feedback)
        {
            try
            {
                await _feedBackService.SaveFeedback(feedback.BookId, feedback.UserId, feedback.Comment);
                await _commentHub.Clients.Group(feedback.BookId.ToString()).SendAsync("ReceivedComment", feedback.UserId, feedback.Comment);
                return Ok();  // Return status code 200 when comment is posted
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }
        [HttpDelete]
        public async Task DeleteFeedback(long feedBackId)
        {
            try
            {
                await _feedBackService.DeleteFeedback(feedBackId);
                Ok();
            }
            catch (Exception)
            {
                StatusCode(500, "Internal server error, Please try again later");
            }
        }
    }
}
