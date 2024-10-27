using BackEnd.Models;

namespace BackEnd.Service
{
    public interface IFeedBackService
    {
        Task<List<Feedback>> GetFeedBack(long bookId);
    }
}
