using BackEnd.Models;

namespace BackEnd.Repository
{
    public interface ICollectionRepository
    {
        Task<Collection> GetCollectionByIdAsync(long id);
        Task<IEnumerable<Collection>> GetAllCollectionsAsync();
        Task SaveCollectionAsync(Collection collection);
        Task UpdateCollectionAsync(Collection collection);
        Task DeleteCollectionAsync(long id);
    }
}
