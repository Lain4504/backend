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
        private static readonly object _lock = new object();
        private readonly IOrderService _OrderService;
        public OrderController(IOrderService OrderService)
        {
            _OrderService = OrderService;
        }



        [HttpGet("user/{id}")]
        public async Task<IActionResult> getOrderByUser(long id)
        {
            try
            {
                List<Order> orders = await _OrderService.GetOrderByUser(id);
                if (orders.Count == 0)
                {
                    return NotFound("userId not exits, please inter correct userId");
                }
                return Ok(orders);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }
        [HttpGet("orderdetail/{orderId}")]
        public async Task<IActionResult> getOrderDetails(long orderId)
        {
            try
            {
                List<OrderDetail> detail = await _OrderService.GetOrderDetail(orderId);
                return Ok(detail);
            }
            catch (Exception)
            {

                return StatusCode(500, "Internal server error, Please try again");
            }
        }
        [HttpPost("process")]
        public async Task processOrder(Order order)
        {
            await _OrderService.ChangeOrderState(order.Id, OrderState.Processing);
            lock (_lock)
            {
                _OrderService.ProcessOrderAsync(order);
            }
        }

        [HttpGet("get-all")]
        public async Task<ActionResult> getAll()
        {
            try
            {
                var Orders = await _OrderService.GetAll();
                if (Orders == null || Orders.Count() == 0)
                {
                    return NoContent();
                }
                return Ok(Orders);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error. Please try again later.");

            }

        }

        [HttpGet("{id}")]
        public Task<Order> getOrderById(long id)
        {
            return _OrderService.GetOrderById(id);
        }

        [HttpPut("update-shipping/{id}/{state}")]
        public async Task<IActionResult> UpdateShipping(long id, ShippingState state)
        {
            try
            {
                await _OrderService.ChangeOrderShippingState(id, state);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }


        [HttpPut("update-orderState/{id}/{state}")]
        public async Task<IActionResult> UpdateOrderState([FromRoute] long id, [FromRoute] OrderState state)
        {
            try
            {
                await _OrderService.ChangeOrderState(id, state);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }


        [HttpPut("cancel/{id}")]
        public async Task<IActionResult> CancelOrder([FromRoute] long id)
        {
            try
            {
                var existingOrder = await _OrderService.GetOrderById(id);
                if (existingOrder.ShippingState == null) throw new Exception("Cannot found shipping state");

                if (existingOrder.ShippingState.Equals(ShippingState.SHIPPING))
                {
                    return Ok("Không thể hủy đơn hàng đang trong quá trình vận chuyển");
                }

                await _OrderService.Cancel(id);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }
    }
}
