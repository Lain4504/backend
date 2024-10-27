using BackEnd.Models;
using BackEnd.Repository;

namespace BackEnd.Service.ServiceImpl
{
    public class AdsService : IAdsService
    {
        private readonly IAdsRepository _repository;

        public AdsService(IAdsRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<Ads>> GetAllAdsAsync()
        {
            return _repository.GetAllAds();
        }

        public async Task<Ads> GetAdsByIdAsync(int id)
        {
            return await _repository.GetAdsById(id);
        }

        public async Task AddAdsAsync(Ads ads)
        {
            if (ads == null)
            {
                throw new ArgumentNullException(nameof(ads), "Ads cannot be null");
            }
            await _repository.AddAds(ads);
        }

        public async Task UpdateAdsAsync(Ads ads)
        {
            if (ads == null)
            {
                throw new ArgumentNullException(nameof(ads), "Ads cannot be null");
            }
            await _repository.UpdateAds(ads);
        }

        public async Task DeleteAdsAsync(int id)
        {
            var existingAds = await _repository.GetAdsById(id);
            if (existingAds == null)
            {
                throw new KeyNotFoundException($"Ads with ID {id} not found");
            }
            await _repository.DeleteAds(id);
        }
    }
}
