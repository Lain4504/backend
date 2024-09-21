using BackEnd.Models;

namespace BackEnd.Repository
{
    public interface ISliderRepository
    {
        Task<IEnumerable<Slider>> GetAllSlider();
    }
}
