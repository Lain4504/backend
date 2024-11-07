
using BackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Repository.RepositoryImpl
{
    public class FeedBackRepository : IFeedBackRepository
    {
        BookStoreContext _context;
        public FeedBackRepository(BookStoreContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteFeedback(long feedbackId)
        {
            var feedBackDelete = await _context.Feedbacks.FindAsync(feedbackId);
            if (feedBackDelete == null)
            {
                return false;
            }
            _context.Feedbacks.Remove(feedBackDelete);
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<Feedback>> GetFeedBack(long bookId)
        {
            var feedBack = await _context.Feedbacks
                  .Where(f => f.BookId == bookId)
                  .OrderByDescending(f => f.CreatedAt)
                  .ToListAsync();
            return feedBack;
        }

        public async Task SaveFeedback(long bookId, long userId, string commentContent)
        {
            var newFeedBack = new Feedback
            {
                BookId = bookId,
                UserId = userId,
                Comment = commentContent,
                CreatedAt = DateTime.Now,
                State = "active"
            };

            await _context.Feedbacks.AddAsync(newFeedBack);
            await _context.SaveChangesAsync();
        }
        public async Task<Feedback> GetFeedbackById(long id)
        {
            return await _context.Feedbacks.FindAsync(id);
        }
    }
}
