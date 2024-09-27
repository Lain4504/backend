using System.Collections.Generic;
using System.Threading.Tasks;
using BackEnd.Models;

namespace BackEnd.Service
{
    public interface IPublisherService
    {
        Task<IEnumerable<Publisher>> GetAllPublishersAsync();
        Task<Publisher> GetPublisherByIdAsync(long id);
        Task CreatePublisherAsync(Publisher publisher);
        Task UpdatePublisherAsync(long id, string newName);
        Task DeletePublisherAsync(long id);
    }
}
