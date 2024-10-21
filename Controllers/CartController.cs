using BackEnd.Models;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("api/cart")]
    [Produces("application/json")]
    [Microsoft.AspNetCore.Cors.EnableCors("AllowSpecificOrigins")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCart()
        {
            try
            {
                var carts =await _cartService.GetAllCart();
                return Ok(carts);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error. Please try again later.");
            }

        }
        
        [HttpGet("by-user/{userId}")]
        public async Task<IActionResult> GetCartByUser(long userId)
        {
            var cart =await  _cartService.GetCartByUser(userId);
            return Ok(cart);
        }

        [HttpPut("")]
        public async Task<IActionResult> UpdateUserCart([FromBody] Order order)
        {
            await _cartService.UpdateCart(order);
            return NoContent();
        }

        [HttpPost("add")]
        public IActionResult AddCart([FromBody] OrderDetail orderDetail)
        {
            _cartService.AddToCart(orderDetail);
            return NoContent();
        }
    }
}
