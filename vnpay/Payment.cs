using BackEnd.Models;
using Microsoft.EntityFrameworkCore;
using VNPAY_CS_ASPX;
using Microsoft.AspNetCore.Http;

public class Payment
{
    private readonly BookStoreContext _context;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public Payment(BookStoreContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string> UrlPayment(int orderId)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
        if (order == null)
        {
            throw new Exception("Order not found");
        }

        // Configuration details
        string vnp_Returnurl = _configuration["VNPay:vnp_Returnurl"];
        string vnp_Url = _configuration["VNPay:vnp_Url"];
        string vnp_TmnCode = _configuration["VNPay:vnp_TmnCode"];
        string vnp_HashSecret = _configuration["VNPay:vnp_HashSecret"];

        VnPayLibrary vnpay = new VnPayLibrary();
        var price = (long)order.TotalPrice;
        vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
        vnpay.AddRequestData("vnp_Command", "pay");
        vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
        vnpay.AddRequestData("vnp_Amount", price.ToString());
        vnpay.AddRequestData("vnp_CreateDate", order.Created.Value.ToString("yyyyMMddHHmmss"));
        vnpay.AddRequestData("vnp_CurrCode", "VND");

        // Use Utils with IHttpContextAccessor for IP address
        Utils utils = new Utils(_httpContextAccessor);
        vnpay.AddRequestData("vnp_IpAddr", utils.GetIpAddress());
        vnpay.AddRequestData("vnp_Locale", "vn");
        vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + order.Id);
        vnpay.AddRequestData("vnp_OrderType", "other");
        vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
        vnpay.AddRequestData("vnp_TxnRef", order.Id.ToString());

        // Generate the payment URL
        string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
        return paymentUrl; // Return the generated payment URL
    }
}
