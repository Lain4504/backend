using BackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Repository.RepositoryImpl
{
    public class SliderRepository : ISliderRepository
    {
        private readonly BookStoreContext _context;
        public SliderRepository(BookStoreContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Slider>> GetAllSlider()
        {
            return await _context.Sliders.ToListAsync();
        }
    }
}
