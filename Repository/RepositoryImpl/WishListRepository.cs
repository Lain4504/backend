using System.Linq;
using System.Threading.Tasks;
using BackEnd.Models;
using BackEnd.Util;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Repository.RepositoryImpl
{
    public class WishlistRepository : IWishListRepository
    {
        private readonly BookStoreContext _context;

        public WishlistRepository(BookStoreContext context)
        {
            _context = context;
        }

        public async Task<List<Wishlist>> GetWishlistByUserAsync(long userId)
        {
            var qr = from w in _context.Wishlists
                     where w.UserId == userId
                     select w;
            return await qr.ToListAsync();
        }

        public async Task AddWishlistAsync(Wishlist wishlist)
        {
            var existingWishlist = await _context.Wishlists
        .FirstOrDefaultAsync(w => w.BookId == wishlist.BookId && w.UserId == wishlist.UserId);

            if (existingWishlist == null)
            {
                _context.Wishlists.Add(wishlist);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteWishlistAsync(long id)
        {
            var Wishlist = await _context.Wishlists.FindAsync(id);
            if (Wishlist != null)
            {
                // Remove the Wishlist
                _context.Wishlists.Remove(Wishlist);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAllWishlistAsync(long userId)
        {
            var qr = from w in _context.Wishlists
                     where w.UserId == userId
                     select w;
            var kq = qr.ToList();
            _context.Wishlists.RemoveRange(qr);
            await _context.SaveChangesAsync();
        }

    }

}
