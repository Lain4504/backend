
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
        public async Task<List<Feedback>> GetFeedBack(long bookId)
        {
            var feedBack = await _context.Feedbacks
                  .Where(f => f.BookId == bookId)
                  .OrderBy(f => f.CreatedAt)
                  .ToListAsync();
            return feedBack;      
        }
    }
}
