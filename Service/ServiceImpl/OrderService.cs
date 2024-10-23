using BackEnd.Models;
using BackEnd.Repository;

namespace BackEnd.Service.ServiceImpl
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBookRepository _bookRepository;
        public OrderService(IOrderRepository orderRepository, IUserRepository userRepository, IBookRepository bookRepository)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _bookRepository = bookRepository;
        }
        public async Task<List<Order>> GetOrderByUser(long userId)
        {
            return await _orderRepository.GetOrderByUserIdAsync(userId);
        }

        public async Task ChangeOrderState(long orderId, OrderState orderState)
        {
            var existingOrder = await _orderRepository.GetOrderByOrderIdAsync(orderId);
            if (existingOrder.State == null) { return; }
            if (!existingOrder.State.Equals(orderState))
            {
                if (orderState == OrderState.Processing)
                    existingOrder.Created = DateTime.Now;

                existingOrder.State = orderState.ToString();
            }

            await _orderRepository.ChangeOrderState(orderId, existingOrder.State);
        }


        public async Task ChangeOrderPaymentState(long orderId, PaymentState paymentState)
        {
            await _orderRepository.ChangeOrderPaymentState(orderId, paymentState);
        }

        public async Task ChangeOrderShippingState(long orderId, ShippingState shippingState)
        {
            await _orderRepository.ChangeOrderShippingState(orderId, shippingState);
        }

        public Task<List<Order>> GetAll()
        {
            return _orderRepository.GetAllOrderStateNotAsync(OrderState.Cart);
        }

        public async Task<Order> GetOrderById(long id)
        {
            return await _orderRepository.GetOrderByOrderIdAsync(id);
        }

        public async Task Cancel(long orderId)
        {
            var existingOrder = await GetOrderById(orderId); // Await the result
            if (existingOrder.State != OrderState.Canceled.ToString()) // Check before changing state
            {
                existingOrder.State = OrderState.Canceled.ToString(); // Update state

                foreach (var orderDetail in existingOrder.OrderDetails)
                {
                    var existingBook = orderDetail.Book;
                    if (existingBook == null) return;
                    existingBook.Stock += orderDetail.Amount; // Restore book stock
                }

            }

            await _orderRepository.Cancel(orderId);
        }

        public async Task ProcessOrderAsync(Order order)
        {
            var existingOrder = await _orderRepository.GetOrderByOrderIdAsync(order.Id)
                ?? throw new Exception("Order not found");

            if (!existingOrder.State.Equals(OrderState.Processing))
                return;

            order.Created = existingOrder.Created;

            var existingUser = await _userRepository.GetByIDAsync(order.User.Id);
            if (existingUser == null)
            {
                return;
            }

            foreach (var orderDetail in order.OrderDetails)
            {
                var existingBook = await _bookRepository.GetByIdAsync(orderDetail.Book.Id);
                if (existingBook == null || !existingBook.State.Equals(BookState.ACTIIVE) || existingBook.Stock < orderDetail.Amount)
                {
                    order.State.Equals(OrderState.Canceled);
                    order.ShopNote = "Đơn hàng đã bị hủy do một số sản phẩm hiện không khả dụng";
                    await _orderRepository.SaveAsync(order);
                    return;
                }
            }

            order.State.Equals(OrderState.Confirmed);
            if (order.State.Equals(OrderState.Confirmed))
            {
                foreach (var orderDetail in order.OrderDetails)
                {
                    var existingBook = await _bookRepository.GetByIdAsync(orderDetail.Book.Id);
                    existingBook.Stock -= orderDetail.Amount;
                }
            }

            await _orderRepository.SaveAsync(order);
        }

        public async Task<List<OrderDetail>> GetOrderDetail(long orderId)
        {
            return await _orderRepository.GetOrderDetail(orderId);
        }
    }
}
