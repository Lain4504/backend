using BackEnd.Models;

namespace BackEnd.Repository
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetOrderByUserIdAsync(long userid);
        Task<Order> GetOrderByOrderIdAsync(long orderId);
        Task<List<Order>> GetAllOrderStateNotAsync(OrderState state);
        Task ChangeOrderPaymentState(long id, PaymentState state);
        Task ChangeOrderState(long id, string state);
        Task ChangeOrderShippingState(long orderId,ShippingState shippingState);
        Task Cancel(long id);
        Task SaveAsync(Order order);
    }
}
