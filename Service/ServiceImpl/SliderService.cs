using BackEnd.Models;
using BackEnd.Repository;

namespace BackEnd.Service.ServiceImpl
{
    public class SliderService : ISliderService
    {
        private readonly ISliderRepository _repository;

        public SliderService(ISliderRepository repository)
        {
            _repository = repository;
        }
        public Task<IEnumerable<Slider>> GetAllSlidersAsync()
        {
            return _repository.GetAllSlider();
        }
    }
}
