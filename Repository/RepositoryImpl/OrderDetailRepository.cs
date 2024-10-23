using BackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Repository.RepositoryImpl
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly BookStoreContext _context;
        public OrderDetailRepository(BookStoreContext context)
        {
            _context = context;
        }

        public async Task AddAsync(OrderDetail orderDetail)
        {
            await _context.OrderDetails.AddAsync(orderDetail);
            await _context.SaveChangesAsync();
        }

        public async Task<OrderDetail?> FindByOrderAndBookAsync(Order order, Book book)
        {
            var result = await _context.OrderDetails.FirstOrDefaultAsync(o => o.Order == order && o.Book == book);
            return result;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}