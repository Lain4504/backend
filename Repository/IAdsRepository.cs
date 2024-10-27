using BackEnd.Models;

namespace BackEnd.Repository
{
    public interface IAdsRepository
    {
        Task<IEnumerable<Ads>> GetAllAds();
        Task<Ads> GetAdsById(int id);
        Task AddAds(Ads ads);
        Task UpdateAds(Ads ads);
        Task DeleteAds(int id);
    }
}
