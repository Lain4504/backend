using BackEnd.DTO.Request;
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
        public async Task<IActionResult> AddCart([FromBody] AddToCartRequest request)
        {
            // Kiểm tra xem request có hợp lệ không
            if (request == null || request.BookId <= 0 || request.Quantity <= 0 || request.UserId <= 0)
            {
                return BadRequest("Thông tin sản phẩm không hợp lệ.");
            }

            try
            {
                // Gọi phương thức AddToCart từ service
                await _cartService.AddToCart(request.BookId, request.Price, request.Quantity, request.UserId);
                return NoContent(); // Trả về 204 No Content nếu thêm giỏ hàng thành công
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ và trả về mã lỗi phù hợp
                return BadRequest(ex.Message);
            }
        }

    }
}
