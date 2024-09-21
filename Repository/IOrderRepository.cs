using BackEnd.Models;

namespace BackEnd.Repository
{
    public interface IOrderRepository
    {
        Task<Order> GetOrderByIdAsync(long id);
        Task<IEnumerable<Order>> GetAllOrderAsync();
        Task SaveOrdernAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(long id);
    }
}
