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

        public async Task<Slider> GetSliderById(long id)  
        {
            // Sử dụng AsNoTracking() để ngăn việc theo dõi các entity khi chỉ muốn lấy dữ liệu
            return await _context.Sliders.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task AddSlider(Slider slider)
        {
            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSlider(Slider slider)
        {
            var existingEntity = _context.Sliders.Local.FirstOrDefault(e => e.Id == slider.Id);
            if (existingEntity != null)
            {
                _context.Entry(existingEntity).State = EntityState.Detached; // Tách entity đã theo dõi
            }

            _context.Sliders.Update(slider);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSlider(long id)  
        {
            var slider = await GetSliderById(id);
            if (slider != null)
            {
                _context.Sliders.Remove(slider);
                await _context.SaveChangesAsync();
            }
        }
    }
}
