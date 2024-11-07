using BackEnd.DTO.Request;
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
        Task ChangeOrderShippingState(long orderId, ShippingState shippingState);
        Task Cancel(long id);
        Task SaveAsync(Order order);
        Task<List<Order>> GetAllAsync();
        Task<Order> GetByUserAndStateAsync(long userId, OrderState state);
        Task<List<Order>> GetAllOrderAndState(OrderState state);
        Task AddNewCartAsync(Order order);

        Task<List<OrderDetail>> GetOrderDetail(long orderId);
        Task UpdateInfoOrder(long orderId, UpdateOrderRequest updateOrder);
        Task UpdateQuantityOrder( List<UpdateQuantityOrder> updateOrder);
        Task DeleteOrder(long order);
    }
}
