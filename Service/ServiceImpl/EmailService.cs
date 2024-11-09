using BackEnd.Models;
using MailKit.Net.Smtp;
using MimeKit;
using System.Text;
using System.Web;

namespace BackEnd.Service.ServiceImpl
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userservice;
        private readonly IJwtService _jWTService;
        public EmailService(IConfiguration configuration, IUserService userService, IJwtService jWTService)
        {
            _configuration = configuration;
            _userservice = userService;
            _jWTService = jWTService;
        }

        // Hàm gửi email kích hoạt
        public async Task SendActivationEmail(string email, string token)
        {
            var activationLink = $"http://localhost:5001/activation/{token}";

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("ForeverBookStore", "lehuynhtuong9a2@gmail.com"));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "Activate your account";
            message.Body = new TextPart("html")
            {
                Text = $@"
        <html>
            <head>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        background-color: #f4f4f4;
                        margin: 0;
                        padding: 0;
                    }}
                    .container {{
                        max-width: 600px;
                        margin: 20px auto;
                        background-color: #ffffff;
                        padding: 20px;
                        border-radius: 8px;
                        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                    }}
                    .button {{
                        display: inline-block;
                        padding: 12px 20px;
                        background-color: #4CAF50;
                        color: white;
                        text-decoration: none;
                        border-radius: 5px;
                        font-weight: bold;
                        text-align: center;
                        margin-top: 20px;
                        transition: background-color 0.3s;
                    }}
                    .button:hover {{
                        background-color: #45a049;
                    }}
                    h2 {{
                        color: #333;
                    }}
                    p {{
                        font-size: 16px;
                        color: #555;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <h2>Welcome to ForeverBookStore!</h2>
                    <p>Thank you for registering with us. To activate your account, please click the button below:</p>
                    <a href='{activationLink}' class='button'>Kích hoạt tài khoản</a>
                    <p>If you did not request this activation, please ignore this email.</p>
                    <p>Best regards,<br>ForeverBookStore Team</p>
                </div>
            </body>
        </html>"
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                client.Authenticate("lehuynhtuong9a2@gmail.com", "oeyj oxyn vbit ohkp");

                try
                {
                    await client.SendAsync(message); // Gửi email
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending email: {ex.ToString()}"); // In thông báo lỗi chi tiết
                }
                finally
                {
                    client.Disconnect(true);
                }
            }
        }

        public async Task SendResetPasswordEmail(string email, long id, string role)
        {
            var token = _jWTService.GenerateJwtToken(email, id, role);
            var resetLink = $"http://localhost:5001/reset-password/{token}";

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("ForeverBookStore", "lehuynhtuong9a2@gmail.com"));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "Reset your password";
            message.Body = new TextPart("html")
            {
                Text = $@"
        <html>
            <head>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        background-color: #f4f4f4;
                        margin: 0;
                        padding: 0;
                    }}
                    .container {{
                        max-width: 600px;
                        margin: 20px auto;
                        background-color: #ffffff;
                        padding: 20px;
                        border-radius: 8px;
                        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                    }}
                    .button {{
                        display: inline-block;
                        padding: 12px 20px;
                        background-color: #4CAF50;
                        color: white;
                        text-decoration: none;
                        border-radius: 5px;
                        font-weight: bold;
                        text-align: center;
                        margin-top: 20px;
                        transition: background-color 0.3s;
                    }}
                    .button:hover {{
                        background-color: #45a049;
                    }}
                    h2 {{
                        color: #333;
                    }}
                    p {{
                        font-size: 16px;
                        color: #555;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <h2>Reset Your Password</h2>
                    <p>We received a request to reset your password. If you requested this, click the button below:</p>
                    <a href='{resetLink}' class='button'>Đặt lại mật khẩu</a>
                    <p>If you did not request a password reset, please ignore this email.</p>
                    <p>Best regards,<br>ForeverBookStore Team</p>
                </div>
            </body>
        </html>"
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                client.Authenticate("lehuynhtuong9a2@gmail.com", "oeyj oxyn vbit ohkp");

                try
                {
                    await client.SendAsync(message); // Gửi email
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending email: {ex.ToString()}"); // In thông báo lỗi chi tiết
                }
                finally
                {
                    client.Disconnect(true);
                }
            }
        }

        public async Task SendOrderConfirmationEmail(string email, Order order)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("ForeverBookStore", "lehuynhtuong9a2@gmail.com"));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "Order Confirmation";

            var bodyBuilder = new BodyBuilder();
            var sb = new StringBuilder();

            sb.AppendLine("<html>");
            sb.AppendLine("<head>");
            sb.AppendLine("<style>");
            sb.AppendLine("body { font-family: Arial, sans-serif; color: #333; line-height: 1.6; }");
            sb.AppendLine("h2 { color: #4CAF50; }");
            sb.AppendLine("table { width: 100%; border-collapse: collapse; margin-top: 20px; }");
            sb.AppendLine("th, td { padding: 12px; text-align: left; border: 1px solid #ddd; }");
            sb.AppendLine("th { background-color: #f2f2f2; font-weight: bold; }");
            sb.AppendLine("tr:nth-child(even) { background-color: #f9f9f9; }");
            sb.AppendLine("tr:hover { background-color: #f1f1f1; }");
            sb.AppendLine(".product-info { display: flex; align-items: center; }");
            sb.AppendLine(".product-info img { border-radius: 8px; width: 60px; height: 60px; object-fit: cover; margin-right: 15px; }");
            sb.AppendLine(".total-row { font-weight: bold; background-color: #e7e7e7; }");
            sb.AppendLine(".order-summary { margin-top: 20px; font-size: 16px; }");
            sb.AppendLine("</style>");
            sb.AppendLine("</head>");

            sb.AppendLine("<body>");
            sb.AppendLine("<h2>Xác nhận đơn hàng</h2>");
            sb.AppendLine($"<p><strong>ID của đơn hàng:</strong> {order.Id}</p>");
            sb.AppendLine($"<p><strong>Tên khách hàng:</strong> {order.FullName}</p>");
            sb.AppendLine($"<p><strong>Số điện thoại:</strong> {order.Phone}</p>");
            sb.AppendLine("<h3>Chi tiết đơn hàng:</h3>");
            sb.AppendLine("<table>");
            sb.AppendLine("<tr><th>Sản phẩm</th><th>Giá</th><th>Số lượng</th><th>Tổng</th></tr>");

            foreach (var item in order.OrderDetails)
            {
                var imageLink = item.Book.Images.FirstOrDefault()?.Link ?? "https://via.placeholder.com/60";
                sb.AppendLine("<tr>");
                sb.AppendLine("<td>");
                sb.AppendLine($"<div class='product-info'>");
                sb.AppendLine($"<img src='{imageLink}' alt='{item.Book.Title}' />");
                sb.AppendLine($"<strong>{item.Book.Title}</strong>");
                sb.AppendLine("</div>");
                sb.AppendLine("</td>");
                sb.AppendLine($"<td>{item.SalePrice?.ToString("N0")} VND</td>");
                sb.AppendLine($"<td>{item.Amount?.ToString("N0")}</td>");
                sb.AppendLine($"<td>{((item.SalePrice ?? 0) * (item.Amount ?? 0)).ToString("N0")} VND</td>");
                sb.AppendLine("</tr>");
            }

            sb.AppendLine("</table>");

            sb.AppendLine("<div class='order-summary'>");
            sb.AppendLine($"<p><strong>Phí vận chuyển:</strong> {order.ShippingPrice?.ToString("N0")} VND</p>");
            sb.AppendLine($"<p><strong>Tổng giá trị của đơn hàng:</strong> {((order.ShippingPrice ?? 0) + order.OrderDetails.Sum(i => (i.SalePrice ?? 0) * (i.Amount ?? 0))).ToString("N0")} VND</p>");
            sb.AppendLine("</div>");

            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            bodyBuilder.HtmlBody = sb.ToString();
            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                client.Authenticate("lehuynhtuong9a2@gmail.com", "oeyj oxyn vbit ohkp");
                try
                {
                    await client.SendAsync(message);
                }
                finally
                {
                    client.Disconnect(true);
                }
            }
        }
    }
}
