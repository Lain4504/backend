using BackEnd.Model;

namespace BackEnd.Repository
{
    public interface ICollectionRepository
    {
        Task<Collection> GetCollectionByIdAsync(int id);
        Task<List<Collection>> GetAllCollectionsAsync();
        Task<Collection> SaveCollectionAsync(Collection collection);
        Task<Collection> UpdateCollectionAsync(Collection collection);
        Task DeleteCollectionAsync(int id);
    }
}
