using BackEnd.Models;
using BackEnd.Util;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Repository.RepositoryImpl
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
            var collection = await _context.Collections.FindAsync(id);
            if (collection != null)
            {
                // Remove all books associated with this collection
                var books = _context.BookCollections.Where(bc => bc.CollectionId == id);
                _context.BookCollections.RemoveRange(books);

                // Remove the collection
                _context.Collections.Remove(collection);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Collection>> GetAllCollectionsAsync()
        {
            return await _context.Collections.ToListAsync();
        }

        public async Task<Collection> GetCollectionByIdAsync(long id)
        {
            return await _context.Collections.FindAsync(id);
        }

        public async Task SaveCollectionAsync(Collection collection)
        {
            await _context.Collections.AddAsync(collection);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCollectionAsync(Collection collection)
        {
            _context.Collections.Update(collection);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Collection>> GetAllBookCollectionsAsync(int pageIndex, int pageSize, string sortBy, bool isAscending)
        {
            var query = _context.Collections.AsQueryable();

            // Apply sorting
            query = isAscending
                ? query.OrderBy(b => EF.Property<object>(b, sortBy))
                : query.OrderByDescending(b => EF.Property<object>(b, sortBy));

            // Apply pagination
            return await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<bool> RemoveCollectionFromBook(long bookId, long collectionId)
        {
            var bookCollection = await _context.BookCollections
                .FirstOrDefaultAsync(bc => bc.BookId == bookId && bc.CollectionId == collectionId);
            if (bookCollection != null)
            {
                _context.BookCollections.Remove(bookCollection);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
