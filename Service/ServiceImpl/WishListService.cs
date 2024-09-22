using BackEnd.Models;
using BackEnd.Repository;
using BackEnd.Util;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Service.ServiceImpl
{
    public class WishlistService : IWishlistService
    {
        private readonly IWishListRepository _repository;
        public WishlistService(IWishListRepository repository)
        {
            _repository = repository;
        }

        public Task<List<Wishlist>> GetWishlistByUserAsync(long userId)
        {
            return _repository.GetWishlistByUserAsync(userId);
        }

        public Task AddWishlistAsync(Wishlist Wishlist)
        {
            return _repository.AddWishlistAsync(Wishlist);
        }

        public Task DeleteAllWishlistAsync(long userId)
        {
            return _repository.DeleteAllWishlistAsync(userId);
        }

        public Task DeleteWishlistAsync(long id)
        {
            return _repository.DeleteWishlistAsync(id);
        }

    }
}