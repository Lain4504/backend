using BackEnd.Models;

namespace BackEnd.Service
{
    public interface IAdsService
    {
        Task<IEnumerable<Ads>> GetAllAdsAsync();
        Task<Ads> GetAdsByIdAsync(int id);
        Task AddAdsAsync(Ads ads);
        Task UpdateAdsAsync(Ads ads);
        Task DeleteAdsAsync(int id);
    }
}
