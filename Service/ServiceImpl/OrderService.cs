using BackEnd.Models;
using BackEnd.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

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

        // public async Task ProcessOrder(Order order)
        // {
        //     var existingOrder = await _orderRepository.GetOrderByOrderIdAsync(order.Id);

        //     if (existingOrder == null || !existingOrder.State.Equals(OrderState.Processing))
        //         return;

        //     order.Created = existingOrder.Created;

        //     var existingUser = await _userRepository.GetByIDAsync(order.UserId ?? 0); // Await the user retrieval
        //     if (existingUser == null)
        //     {
        //         return;
        //     }

        //     foreach (var orderDetail in order.OrderDetails)
        //     {
        //         var existingBook = await _bookRepository.GetByIdAsync(orderDetail.Book.Id);
        //         if (existingBook == null || !existingBook.State.Equals(BookState.ACTIIVE) || existingBook.Stock < orderDetail.Amount)
        //         {
        //             order.State = OrderState.Canceled.ToString(); // Correctly assign the state
        //             order.ShopNote = "Đơn hàng đã bị hủy do một số sản phẩm hiện không khả dụng";
        //             await _orderRepository.UpdateOrderAsync(order); // Save the changes to the order
        //             return;
        //         }
        //     }
        // }


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


        public Task<List<Order>> QueryOrder(OrderState state, PaymentState paymentState, ShippingState shippingState, DateTime from, DateTime to, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
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
            // var existingOrder = await GetOrderById(orderId); // Await the result
            // if (existingOrder.State != OrderState.Canceled.ToString()) // Check before changing state
            // {
            //     existingOrder.State = OrderState.Canceled.ToString(); // Update state

            //     foreach (var orderDetail in existingOrder.OrderDetails)
            //     {
            //         var existingBook = orderDetail.Book;
            //         if (existingBook == null) return;
            //         existingBook.Stock += orderDetail.Amount; // Restore book stock
            //     }

            // }

            await _orderRepository.Cancel(orderId);
        }


    }
}
