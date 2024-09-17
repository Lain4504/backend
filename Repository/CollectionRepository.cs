using BackEnd.Exceptions;
using BackEnd.Model;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Repository
{
    public class CollectionRepository : ICollectionRepository
    {
        private readonly BookStoreContext _context;
        public CollectionRepository(BookStoreContext context)
        {
            _context = context;
        }

        public async Task DeleteCollectionAsync(long id)
        {
            var collection = await _context.Collections
                .Include(collection => collection.Books)
                .FirstOrDefaultAsync(collection => collection.Id == id);
            if (collection == null)
            {
                throw new MessageException("Collection not found");
            }
                foreach (var book in collection.Books)
                {
                    book.Collections.Remove(collection);
                }
                _context.Collections.Remove(collection);
                await _context.SaveChangesAsync();
        }

        public async Task<List<Collection>> GetAllCollectionsAsync()
        {
           return await _context.Collections
                .Include(collection => collection.Books)
                .ToListAsync();
        }

        public async Task<Collection> GetCollectionByIdAsync(long id)
        {
            var collection = await _context.Collections
                .Include(collection => collection.Books)
                .FirstOrDefaultAsync(collection => collection.Id == id);

            if (collection == null)
            {
                throw new KeyNotFoundException($"Collection with id {id} not found.");
            }

            return collection;
        }


        public async Task<Collection> SaveCollectionAsync(Collection collection)
        {
            _context.Collections.Add(collection);
            await _context.SaveChangesAsync();
            return collection;
        }

        public async Task<Collection> UpdateCollectionAsync(Collection collection)
        {
           _context.Collections.Update(collection);
            await _context.SaveChangesAsync();
            return collection;
        }
    }
}
