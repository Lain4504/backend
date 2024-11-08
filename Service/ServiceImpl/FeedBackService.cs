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

        public async Task<bool> DeleteFeedback(long feedbackId)
        {
            return await _repository.DeleteFeedback(feedbackId);
        }

        public async Task<List<Feedback>> GetFeedBack(long bookId)
        {
            return await _repository.GetFeedBack(bookId);
        }
        public async Task SaveFeedback(long bookId, long userId, string commentContent)
        {
            await _repository.SaveFeedback(bookId, userId, commentContent);
        }
        public async Task<Feedback> GetFeedbackById(long id)
        {
            return await _repository.GetFeedbackById(id);
        }
    }

}
