using BackEnd.Models;

namespace BackEnd.Service
{
    public interface IOrderService
    {

        Task<Order> GetOrderByIdAsync(long id);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task SaveOrderAsync(Order Order);
        Task UpdateOrderAsync(Order Order);
        Task DeleteOrderAsync(long id);

    }
}
