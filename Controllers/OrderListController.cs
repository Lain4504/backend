using BackEnd.Models;
using BackEnd.Service;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("/api/order")]
    [ApiController]
    [EnableCors("AllowSpecificOrigins")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _OrderService;
        public OrderController(IOrderService OrderService)
        {
            _OrderService = OrderService;
        }
        [HttpGet("get-all")]
        public async Task<ActionResult> GetBookOrders()
        {
            try
            {
                var Orders = await _OrderService.GetAllOrdersAsync();
                if (Orders == null || Orders.Count() == 0)
                {
                    return NoContent();
                }
                return Ok(Orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error. Please try again later.");

            }

        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetOrderById(long id)
        {
            try
            {
                var Order = await _OrderService.GetOrderByIdAsync(id);
                if (Order == null)
                {
                    return NotFound();
                }
                return Ok(Order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error. Please try again later.");
            }


        }
        [HttpPost]
        public async Task<ActionResult> SaveOrder([FromBody] Order Order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _OrderService.SaveOrderAsync(Order); ;
            return CreatedAtAction(nameof(GetOrderById), new { id = Order.Id }, Order);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrder(long id, [FromBody] string newStatus)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _OrderService.UpdateOrderAsync(id, newStatus);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(long id)
        {
            var Order = await _OrderService.GetOrderByIdAsync(id);
            if (Order == null)
            {
                return NotFound();
            }

            await _OrderService.DeleteOrderAsync(id);
            return NoContent();
        }

    }
}
