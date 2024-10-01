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

        public async Task UpdatePublisherAsync(Publisher publisher)
        {
            await _publisherRepository.UpdatePublisherAsync(publisher);
        }

        public async Task DeletePublisherAsync(long id)
        {
            // Step 1: Retrieve books associated with this publisher
            var books = await _publisherRepository.GetBooksByPublisherIdAsync(id);
            if (books != null && books.Any())
            {
                // Step 2: Remove publisher reference from each book
                foreach (var book in books)
                {
                    book.PublisherId = null;
                    await _publisherRepository.UpdateBookAsync(book);
                }
            }

            // Step 3: Delete the publisher
            await _publisherRepository.DeletePublisherAsync(id);
        }
    }
}
