using BackEnd.Models;
namespace BackEnd.Repository
{
    public interface IOrderDetailRepository
    {
        Task<OrderDetail?> FindByOrderAndBookAsync(Order order, Book book);
        Task AddAsync(OrderDetail orderDetail);
        Task SaveChangesAsync();
    }
}
