using BackEnd.Models;
using BackEnd.Util;

namespace BackEnd.Repository
{
    public interface ICollectionRepository
    {
        Task<Collection> GetCollectionByIdAsync(long id);
        Task<IEnumerable<Collection>> GetAllCollectionsAsync();
        Task SaveCollectionAsync(Collection collection);
        Task UpdateCollectionAsync(Collection collection);
        Task DeleteCollectionAsync(long id);
        Task<bool> RemoveCollectionFromBook(long bookId, long collectionId);
        Task<IEnumerable<Collection>> GetAllBookCollectionsAsync(int pageIndex, int pageSize, string sortBy, bool isAscending);
    }
}
