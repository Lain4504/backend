using BackEnd.DTO.Request;
using BackEnd.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

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
                     where o.UserId == userid && o.State != OrderState.Cart.ToString()
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

        public async Task<List<Order>> GetAllAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<Order> GetByUserAndStateAsync(long userId, OrderState state)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails) // Include OrderDetails
                    .ThenInclude(od => od.Book) // Include Book related to OrderDetail
                        .ThenInclude(b => b.Images) // Include Images related to Book
                .FirstOrDefaultAsync(o => o.State == state.ToString() && o.UserId == userId);

            return order;
        }


        public async Task<List<Order>> GetAllOrderAndState(OrderState state)
        {
            var qr = from o in _context.Orders
                     where o.State.Equals(state.ToString())
                     select o;
            return await qr.ToListAsync();
        }

        public async Task AddNewCartAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task<List<OrderDetail>> GetOrderDetail(long orderId)
        {
            var qr = from d in _context.OrderDetails
                     where d.OrderId == orderId
                     select d;
            return await qr.ToListAsync();
        }

        public async Task UpdateInfoOrder(long orderId, UpdateOrderRequest updateOrder)
        {
            //find order by Id
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return;
            }
            //update order
            order.FullName = updateOrder.Name;
            order.Phone = updateOrder.Phone;
            order.Address = updateOrder.Address;
            await _context.SaveChangesAsync();
        }
    }
}
