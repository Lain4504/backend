using BackEnd.Models;
using BackEnd.Service;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("/api/wishlist")]
    [ApiController]
    [EnableCors("AllowSpecificOrigins")]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _WishlistService;
        public WishlistController(IWishlistService WishlistService)
        {
            _WishlistService = WishlistService;
        }
        
        [HttpPost]
        public async Task AddWishList([FromBody] Wishlist wishlist)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState); 
            }
            await _WishlistService.AddWishlistAsync(wishlist); ;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult> GetBookWishlists(long userId)
        {
            try
            {
                var Wishlists = await _WishlistService.GetWishlistByUserAsync(userId);
                if (Wishlists == null || Wishlists.Count() == 0)
                {
                    return NoContent();
                }
                return Ok(Wishlists);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error. Please try again later.");

            }

        }

        //delete each book in wishlist
        [HttpDelete("{id}")]    //id of wishlist in data
        public async Task<IActionResult> DeleteWishlist(long id)
        {
            await _WishlistService.DeleteWishlistAsync(id);
            return NoContent();
        }

        //delete all book in wishlist
        [HttpDelete("all-{userId}")]    //id of user 
        public async Task<IActionResult> DeleteAllWishlist(long userId)
        {
            await _WishlistService.DeleteAllWishlistAsync(userId);
            return NoContent();
        }

    }
}
