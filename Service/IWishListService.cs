using BackEnd.Models;
using BackEnd.Util;

namespace BackEnd.Service
{
    public interface IWishlistService
    {

        Task<List<Wishlist>> GetWishlistByUserAsync(long userId);
        Task AddWishlistAsync(Wishlist Wishlist);
        Task DeleteWishlistAsync(long id);
        Task DeleteAllWishlistAsync(long userId);
    }
}
