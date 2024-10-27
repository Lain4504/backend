
using BackEnd.Models;

namespace BackEnd.Repository
{
    public interface IFeedBackRepository
    {
        Task<List<Feedback>> GetFeedBack(long bookId);
    }
}
