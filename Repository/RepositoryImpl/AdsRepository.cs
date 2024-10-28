using BackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Repository.RepositoryImpl
{
    public class AdsRepository : IAdsRepository
    {
        private readonly BookStoreContext _context;

        public AdsRepository(BookStoreContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ads>> GetAllAds()
        {
            return await _context.Ads.ToListAsync();
        }

        public async Task<Ads> GetAdsById(int id)
        {
            return await _context.Ads.FindAsync(id);
        }

        public async Task AddAds(Ads ads)
        {
            ads.Id = 0;
            await _context.Ads.AddAsync(ads);
            await _context.SaveChangesAsync();
        }

         public async Task UpdateAds(Ads ads)
        {
            var existingEntity = await _context.Ads.FindAsync(ads.Id);
            if (existingEntity != null)
            {
                // Update the tracked entity's values
                _context.Entry(existingEntity).CurrentValues.SetValues(ads);
                await _context.SaveChangesAsync();
            }
            else
            {
                // If not found, this could be a new entity or a problem
                throw new KeyNotFoundException($"Ads with ID {ads.Id} not found");
            }
        }

        public async Task DeleteAds(int id)
        {
            var ads = await GetAdsById(id);
            if (ads != null)
            {
                _context.Ads.Remove(ads);
                await _context.SaveChangesAsync();
            }
        }
    }
}
