using System.Collections.Generic;
using System.Threading.Tasks;
using BackEnd.Models;
using BackEnd.Repository;

namespace BackEnd.Service.ServiceImpl
{
    public class PublisherService : IPublisherService
    {
        private readonly IPublisherRepository _publisherRepository;

        public PublisherService(IPublisherRepository publisherRepository)
        {
            _publisherRepository = publisherRepository;
        }

        public async Task<IEnumerable<Publisher>> GetAllPublishersAsync()
        {
            return await _publisherRepository.GetAllPublishersAsync();
        }

        public async Task<Publisher> GetPublisherByIdAsync(long id)
        {
            return await _publisherRepository.GetPublisherByIdAsync(id);
        }

        public async Task CreatePublisherAsync(Publisher publisher)
        {
            await _publisherRepository.SavePublisherAsync(publisher);
        }

        public async Task UpdatePublisherAsync(long id, string newName)
        {
            var publisher = await _publisherRepository.GetPublisherByIdAsync(id);
            if (publisher != null)
            {
                publisher.Name = newName;
                await _publisherRepository.UpdatePublisherAsync(id, newName);
            }
        }

        public async Task DeletePublisherAsync(long id)
        {
            await _publisherRepository.DeletePublisherAsync(id);
        }
    }
}
