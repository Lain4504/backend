using BackEnd.Models;
using BackEnd.Repository;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Service.ServiceImpl
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        public OrderService(IOrderRepository repository)
        {
            _repository = repository;
        }

        public Task DeleteOrderAsync(long id)
        {
            return _repository.DeleteOrderAsync(id);
        }

        public Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return _repository.GetAllOrderAsync();
        }

        public Task<Order> GetOrderByIdAsync(long id)
        {
            return _repository.GetOrderByIdAsync(id);
        }

        public Task SaveOrderAsync(Order Order)
        {
            return _repository.SaveOrdernAsync(Order);
        }

        public Task UpdateOrderAsync(long id, string newStatus)
        {
            return _repository.UpdateOrderAsync(id, newStatus);
        }
    }
}