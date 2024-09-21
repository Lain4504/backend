using BackEnd.Models;

namespace BackEnd.Service
{
    public interface ISliderService
    {
        Task<IEnumerable<Slider>> GetAllSlidersAsync();

    }
}
