using System.Linq;
using System.Threading.Tasks;
using BackEnd.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BackEnd.Repository.RepositoryImpl
{
    public class OrderRepository : IOrderRepository
    {
        private readonly BookStoreContext _context;

        public OrderRepository(BookStoreContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetOrderByUserIdAsync(long userid)
        {
            var qr = from o in _context.Orders
                     where o.UserId == userid
                     select o;
            var result = await qr.ToListAsync();
            return result;
        }


        public async Task<Order> GetOrderByOrderIdAsync(long orderId)
        {

            var result = await _context.Orders.FindAsync(orderId);
            if (result == null) throw new Exception("Not found");
            return result;
        }

        public async Task<List<Order>> GetAllOrderStateNotAsync(OrderState state)
        {
            var qr = from o in _context.Orders
                     where o.State != state.ToString()
                     select o;
            return await qr.ToListAsync();
        }

        // public Task UpdateOrderAsync(Order order)
        // {
        //     throw new NotImplementedException();
        // }

        public async Task ChangeOrderPaymentState(long id, PaymentState state)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return;
            order.PaymentState = state.ToString();
            await _context.SaveChangesAsync();
        }
        public async Task ChangeOrderState(long id, string state)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return;
            order.State = state;
            await _context.SaveChangesAsync();
        }

        public async Task ChangeOrderShippingState(long orderId, ShippingState shippingState)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return;
            order.ShippingState = shippingState.ToString();
            await _context.SaveChangesAsync();
        }

        public async Task Cancel(long id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return;
            order.State = OrderState.Canceled.ToString();
            await _context.SaveChangesAsync();
        }

        public async Task SaveAsync(Order order)
        {
            await _context.SaveChangesAsync();
        }
    }
}
