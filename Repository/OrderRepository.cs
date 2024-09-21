using System.Linq;
using System.Threading.Tasks;
using BackEnd.Models;
using BackEnd.Repository;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly BookStoreContext _context;

        public OrderRepository(BookStoreContext context)
        {
            _context = context;
        }

        public async Task DeleteOrderAsync(long id)
        {
            var Order = await _context.Orders.FindAsync(id);
            if (Order != null)
            {
                // Remove the Order
                _context.Orders.Remove(Order);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Order>> GetAllOrderAsync()
        {
            return await _context.Orders.ToListAsync();
        }


        public async Task<Order> GetOrderByIdAsync(long id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task SaveOrderAsync(Order Order)
        {
            await _context.Orders.AddAsync(Order);
            await _context.SaveChangesAsync();
        }

        public Task SaveOrdernAsync(Order order)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateOrderAsync(long id, string newStatus)
        {
            var order = await _context.Orders.FindAsync(id);
            order.State = newStatus;
            await _context.SaveChangesAsync();
        }
    }
}
