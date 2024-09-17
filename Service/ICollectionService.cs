using BackEnd.Models;

namespace BackEnd.Service
{
    public interface ICollectionService
    {

        Task<Collection> GetCollectionByIdAsync(long id);
        Task<IEnumerable<Collection>> GetAllCollectionsAsync();
        Task SaveCollectionAsync(Collection collection);
        Task UpdateCollectionAsync(Collection collection);
        Task DeleteCollectionAsync(long id);

    }
}
