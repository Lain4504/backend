using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Models;
using Microsoft.EntityFrameworkCore;

public interface IOrderService
{
    Task<List<Order>> GetOrderByUser(long userId);

    // Task ProcessOrder(Order order);

    Task ChangeOrderState(long orderId, OrderState orderState);

    Task<List<Order>> GetAll();

    Task ChangeOrderPaymentState(long orderId, PaymentState paymentState);

    Task ChangeOrderShippingState(long orderId, ShippingState shippingState);

    Task<Order> GetOrderById(long id);

    Task Cancel(long orderId);
    Task ProcessOrderAsync(Order order);
}
