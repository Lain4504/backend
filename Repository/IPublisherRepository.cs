using System.Collections.Generic;
using System.Threading.Tasks;
using BackEnd.Models;

namespace BackEnd.Repository
{
   public interface IPublisherRepository
{
    Task DeletePublisherAsync(long id);
    Task<IEnumerable<Publisher>> GetAllPublishersAsync();
    Task<Publisher?> GetPublisherByIdAsync(long id);
    Task SavePublisherAsync(Publisher publisher);
    Task UpdatePublisherAsync(long id, string newName);
}
}