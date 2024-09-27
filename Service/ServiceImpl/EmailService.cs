using MailKit.Net.Smtp;
using MimeKit;

namespace BackEnd.Service.ServiceImpl
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userservice;
        public EmailService(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userservice = userService;
        }

        // Hàm gửi email kích hoạt
        public async Task SendActivationEmail(string email, string token)
        {
            var activationLink = $"http://localhost:5173/activation/{token}";

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("ForeverBookStore", "lehuynhtuong9a2@gmail.com"));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "Activate your account";
            message.Body = new TextPart("html")
            {
                Text = $"Click the link to activate your account: <a href='{activationLink}' style='display: inline-block; padding: 10px 15px; background-color: #4CAF50; color: white; text-decoration: none; border-radius: 5px; font-weight: bold; transition: background-color 0.3s;'>Kích hoạt tài khoản / Activate Account</a>"
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

        public async Task SendResetPasswordEmail(string email)
        {
            var token = _userservice.GenerateJwtToken(email);
            var resetLink = $"http://localhost:5173/reset-password/{token}";

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("ForeverBookStore", "lehuynhtuong9a2@gmail.com"));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "Activate your account";
            message.Body = new TextPart("html")
            {
                Text = $"Click the link to reset your password: <a href='{resetLink}' style='display: inline-block; padding: 10px 15px; background-color: #4CAF50; color: white; text-decoration: none; border-radius: 5px; font-weight: bold; transition: background-color 0.3s;'>Đặt lại mật khẩu  / Reset Password</a>"
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
    }
}
