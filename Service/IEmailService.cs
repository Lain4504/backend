using BackEnd.Models;

namespace BackEnd.Service
{
    public interface IEmailService
    {
        Task SendActivationEmail(string email, string token);
        Task SendResetPasswordEmail(string email, long id, string role);
        Task SendOrderConfirmationEmail(string email, Order order);

    }
}
