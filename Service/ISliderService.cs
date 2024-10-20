using BackEnd.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackEnd.Service
{
    public interface ISliderService
    {
        Task<IEnumerable<Slider>> GetAllSlidersAsync();
        Task<Slider> GetSliderByIdAsync(long id);  
        Task AddSliderAsync(Slider slider);
        Task UpdateSliderAsync(Slider slider);
        Task DeleteSliderAsync(long id);  
    }
}
