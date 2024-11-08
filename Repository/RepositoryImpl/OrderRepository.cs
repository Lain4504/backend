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

        public async Task<List<Order>> GetOrderByUserIdAsync(long userId)
        {
            // Use Include to load related OrderDetails
            var qr = from o in _context.Orders
                     .Include(o => o.OrderDetails) // Include OrderDetails in the query
                     where o.UserId == userId && o.State != OrderState.Cart.ToString()
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
        public async Task ChangeOrderState( long id, string state)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return;
            order.State = state;
            var ListOrderDetail =await _context.OrderDetails.Where(f => f.OrderId == order.Id).ToListAsync();
            foreach (var orderDetail in ListOrderDetail)
            {
                var numberBooktoBy = orderDetail.Amount;
                var book =await _context.Books.Where(b => b.Id == orderDetail.BookId).FirstOrDefaultAsync();
                book.Stock = book.Stock - numberBooktoBy;
            }
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
            //Find order detail for order
            // var orderDetailForBookInCart =await (from o in _context.OrderDetails
            //                                where o.OrderId == order.Id
            //                                select o).FirstOrDefaultAsync();
            // //Find book in cart
            // var typeBookInCart =await _context.Books.FindAsync(orderDetailForBookInCart.BookId);
            // typeBookInCart.Stock = typeBookInCart.Stock -1;     //set stock -1
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
            order.TotalPrice = updateOrder.TotalPrice;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateQuantityOrder(List<UpdateQuantityOrder> updateOrders)
        {
            // Ensure the updateOrders list is not empty
            if (updateOrders == null || updateOrders.Count == 0)
                return;

            // Group update orders by OrderId to process each order separately
            var ordersGrouped = updateOrders.GroupBy(uo => uo.orderId);

            foreach (var orderGroup in ordersGrouped)
            {
                var orderId = orderGroup.Key;
                var order = await _context.Orders.FindAsync(orderId);
                if (order == null)
                    throw new Exception($"Order with ID {orderId} not found");

                // Initialize total price for the current order
                long? totalPrice = 0;

                foreach (var updateOrder in orderGroup)
                {
                    // Find the specific OrderDetail using orderDetailId
                    var orderDetail = await _context.OrderDetails
                        .Where(od => od.OrderId == updateOrder.orderId && od.Id == updateOrder.orderDetailId)
                        .FirstOrDefaultAsync();

                    if (orderDetail != null)
                    {
                        var book = await _context.Books.FindAsync(orderDetail.BookId);
                        var stockOfBook = book.Stock;
                        // Update the quantity
                        orderDetail.Amount = updateOrder.quantity;

                        // Recalculate total price based on the item's sale price
                        totalPrice += updateOrder.SalePrice * updateOrder.quantity;

                        //Update stock for book

                    }
                }

                // Update the order's total price
                order.TotalPrice = totalPrice;
            }

            // Save all changes at once
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrder(long orderDetailId)
        {
            // Find the orderdetail by orderDetailId
            var orderDetailsToDelete = await _context.OrderDetails.FindAsync(orderDetailId);
            if (orderDetailsToDelete == null)
                throw new Exception("Order not found");
            _context.OrderDetails.Remove(orderDetailsToDelete);
            var bookInOrder = await _context.Books.FindAsync(orderDetailsToDelete.BookId);
            bookInOrder.Stock = bookInOrder.Stock + orderDetailsToDelete.Amount;
            await _context.SaveChangesAsync();
        }

    }
}
