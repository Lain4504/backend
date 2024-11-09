using BackEnd.DTO.Request;
using BackEnd.Models;
using BackEnd.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers
{

    [Route("/api/order")]
    [ApiController]
    [EnableCors("AllowSpecificOrigins")]
    public class OrderController : ControllerBase
    {
        private static readonly object _lock = new object();
        private readonly IOrderService _orderService;
        private readonly IEmailService _emailService;

        public OrderController(IOrderService OrderService, IEmailService emailService)
        {
            _orderService = OrderService;
            _emailService = emailService;
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> getOrderByUser(long id)
        {
            try
            {
                List<Order> orders = await _orderService.GetOrderByUser(id);
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
                List<OrderDetail> detail = await _orderService.GetOrderDetail(orderId);
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
            await _orderService.ChangeOrderState(order.Id, OrderState.Processing);
            lock (_lock)
            {
                _orderService.ProcessOrderAsync(order);
            }
            await _emailService.SendOrderConfirmationEmail(order.Email, order);
        }
        [Authorize(Policy = "AdminRole")]
        [HttpGet("get-all")]
        public async Task<ActionResult> getAll()
        {
            try
            {
                var Orders = await _orderService.GetAll();
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
            return _orderService.GetOrderById(id);
        }

        [HttpPut("update-shipping/{id}/{state}")]
        public async Task<IActionResult> UpdateShipping(long id, ShippingState state)
        {
            try
            {
                await _orderService.ChangeOrderShippingState(id, state);
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
                await _orderService.ChangeOrderState(id, state);
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
                var existingOrder = await _orderService.GetOrderById(id);
                if (existingOrder.ShippingState == null) throw new Exception("Cannot found shipping state");

                if (existingOrder.ShippingState.Equals(ShippingState.SHIPPING))
                {
                    return Ok("Không thể hủy đơn hàng đang trong quá trình vận chuyển");
                }

                await _orderService.Cancel(id);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }
        [HttpPut("update-info/{id}")]
        public async Task<IActionResult> UpdateInfoOrder(long id, [FromBody] UpdateOrderRequest updatedOrder)
        {
            if (id != updatedOrder.Id)
            {
                return BadRequest("ID không khớp với Order");
            }
            try
            {
                await _orderService.UpdateOrderInfo(id, updatedOrder);
                return Ok(true);
            }
            catch (Exception ex)
            {
                // Trả về mã trạng thái 500 kèm thông điệp lỗi
                return StatusCode(500, $"Lỗi cập nhật đơn hàng: {ex.Message}");
            }
        }

        [HttpPut("update-quantity")]
        public async Task<IActionResult> UpdateQuantityOrder([FromBody] List<UpdateQuantityOrder> updatedOrder)
        {
            // if (id != updatedOrder.Id)
            // {
            //     return BadRequest("ID không khớp với Order");
            // }
            try
            {
                await _orderService.UpdateQuantityorder(updatedOrder);
                return Ok(true);
            }
            catch (Exception ex)
            {
                // Trả về mã trạng thái 500 kèm thông điệp lỗi
                return StatusCode(500, $"Lỗi cập nhật đơn hàng: {ex.Message}");
            }
        }


        [HttpDelete("delete/{orderDetailId}")]
        public async Task<IActionResult> DeleteOrder(long orderDetailId)
        {
            try
            {
                await _orderService.DeleteOrder(orderDetailId);
                return Ok(true);
            }
            catch (Exception ex)
            {
                // Trả về mã trạng thái 500 kèm thông điệp lỗi
                return StatusCode(500, $"Lỗi cập nhật đơn hàng: {ex.Message}");
            }
        }

    }
}
