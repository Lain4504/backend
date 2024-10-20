using BackEnd.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackEnd.Repository
{
    public interface ISliderRepository
    {
        Task<IEnumerable<Slider>> GetAllSlider();
        Task<Slider> GetSliderById(long id);  
        Task AddSlider(Slider slider);
        Task UpdateSlider(Slider slider);
        Task DeleteSlider(long id);  
    }
}
