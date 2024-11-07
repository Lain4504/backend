using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BackEnd.Controllers 
{
    [Route("api/vnpay")]
    [ApiController]
    [EnableCors("AllowSpecificOrigins")]
    public class PaymentController : ControllerBase
    {
        private readonly Payment _paymentService; 

        public PaymentController(Payment paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("url/{orderId}")]
        public async Task<IActionResult> GetPaymentUrl(int orderId)
        {
            try
            {
                // Call the UrlPayment method to generate the payment URL
                string paymentUrl = await _paymentService.UrlPayment(orderId);
                return Ok(paymentUrl); // Return the payment URL
            }
            catch (Exception ex)
            {
                // Handle exceptions and return an appropriate error response
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
