
using BackEnd.Models;

namespace BackEnd.Repository
{
    public interface IFeedBackRepository
    {
        Task<List<Feedback>> GetFeedBack(long bookId);
        Task SaveFeedback(long bookId, long userId, string commentContent);
        Task<bool> DeleteFeedback(long feedbackId);
        Task<Feedback> GetFeedbackById(long id);

    }
}
