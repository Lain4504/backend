using BackEnd.DTO.Request;
using BackEnd.Models;

public interface IOrderService
{
    Task<List<Order>> GetOrderByUser(long userId);
    Task<List<OrderDetail>> GetOrderDetail(long orderId);
    Task ChangeOrderState(long orderId, OrderState orderState);

    Task<List<Order>> GetAll();

    Task ChangeOrderPaymentState(long orderId, PaymentState paymentState);

    Task ChangeOrderShippingState(long orderId, ShippingState shippingState);

    Task<Order> GetOrderById(long id);

    Task Cancel(long orderId);
    Task ProcessOrderAsync(Order order);
    Task UpdateOrderInfo(long orderId, UpdateOrderRequest updateOrder);
    Task UpdateQuantityorder( List<UpdateQuantityOrder> updateOrder);
}
