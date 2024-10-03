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

        // [HttpPost("/process")]
        // public async Task processOrder(long id, Order order)
        // {
        //     await _OrderService.ChangeOrderState(order.Id, OrderState.Processing);
        // }



        //     [HttpGet("")]
        //     public async Task<IActionResult> QueryOrder(
        // [FromRoute] OrderState? state,
        // [FromRoute] ShippingState? shipping,
        // [FromRoute] PaymentState? payment,
        // [FromRoute] DateTime? fromDate,
        // [FromRoute] DateTime? toDate,
        // [FromQuery(Name = "sortBy")] string sortBy = "id",
        // [FromQuery(Name = "page")] int page = 0,
        // [FromQuery(Name = "size")] int size = 5,
        // [FromQuery(Name = "sortOrder")] string sortOrder = "asc")
        //     {
        //         try
        //         {
        //             // Set the sorting direction based on sortOrder
        //             var direction = (sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase))
        //                 ? SortDirection.Ascending
        //                 : SortDirection.Descending;

        //             // Create pageable object using skip and take (for pagination)
        //             var pageable = new Pageable(page, size, direction, sortBy);

        //             // Convert fromDate and toDate to DateTime objects
        //             DateTime? from = null;
        //             DateTime? to = null;

        //             if (fromDate.HasValue)
        //             {
        //                 from = fromDate.Value.Date; // Start of the day
        //             }
        //             if (toDate.HasValue)
        //             {
        //                 to = toDate.Value.Date.AddDays(1).AddTicks(-1); // End of the day
        //             }

        //             // Query the service to get paginated orders
        //             var orders = await _OrderService.QueryOrder(state, payment, shipping, from, to, pageable);

        //             // Return OK response with orders
        //             return Ok(orders);
        //         }
        //         catch (Exception ex)
        //         {
        //             // Handle any exceptions and return server error
        //             return StatusCode(500, "Internal server error. Please try again later.");
        //         }
        //     }



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
