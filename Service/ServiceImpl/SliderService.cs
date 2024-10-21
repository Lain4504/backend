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

        public async Task<Slider> GetSliderByIdAsync(long id)  
        {
            return await _repository.GetSliderById(id);
        }

        public async Task AddSliderAsync(Slider slider)
        {
            if (slider == null)
            {
                throw new ArgumentNullException(nameof(slider), "Slider cannot be null");
            }
            await _repository.AddSlider(slider);
        }

        public async Task UpdateSliderAsync(Slider slider)
        {
            if (slider == null)
            {
                throw new ArgumentNullException(nameof(slider), "Slider cannot be null");
            }

            await _repository.UpdateSlider(slider);
        }

        public async Task DeleteSliderAsync(long id)  
        {
            var existingSlider = await _repository.GetSliderById(id);
            if (existingSlider == null)
            {
                throw new KeyNotFoundException($"Slider with ID {id} not found");
            }
            await _repository.DeleteSlider(id);
        }
    }
}
