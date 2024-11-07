using BackEnd.Models;
using BackEnd.Repository;
namespace BackEnd.Service.ServiceImpl
{
    public class FeedBackService : IFeedBackService
    {
        IFeedBackRepository _repository;
        public FeedBackService(IFeedBackRepository repository)
        {
            _repository = repository;
        }

        public async Task DeleteFeedback(long feedbackId)
        {
            await _repository.DeleteFeedback(feedbackId);
        }

        public async Task<List<Feedback>> GetFeedBack(long bookId)
        {
            return await _repository.GetFeedBack(bookId);
        }
        public async Task SaveFeedback(long bookId, long userId, string commentContent)
        {
            await _repository.SaveFeedback(bookId, userId, commentContent);
        }
    }

}
