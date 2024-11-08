using BackEnd.Models;
using BackEnd.Repository;
using BackEnd.Repository.RepositoryImpl;

namespace BackEnd.Service.ServiceImpl
{
    public class CartService : ICartService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;


        public CartService(
            IOrderRepository orderRepository,
            IUserRepository userRepository,
            IBookRepository bookRepository,
            IOrderDetailRepository orderDetailRepository
            )
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _bookRepository = bookRepository;
            _orderDetailRepository = orderDetailRepository;
        }

        private async Task<User> GetExistingUserAsync(long userId)
        {
            var user = await _userRepository.GetByIDAsync(userId);
            if (user == null)
            {
                throw new Exception("Không tìm thấy user tương ứng");
            }
            return user;
        }

        private async Task<Book> GetExistingBookAsync(long bookId)
        {
            var book = await _bookRepository.GetByIdAsync(bookId);
            if (book == null)
            {
                throw new Exception("Không tìm thấy sách tương ứng");
            }
            return book;
        }

        public async Task AddToCart(int bookId, decimal price, int quantity, int userId)
        {
            // Lấy giỏ hàng hiện tại của người dùng
            var existingCart = await GetCartByUser(userId);

            // Lấy thông tin sách dựa trên bookId
            var existingBook = await GetExistingBookAsync(bookId);
            Console.WriteLine(existingBook.Stock);
            // Tìm kiếm sách trong giỏ hàng
            var bookInCart = await _orderDetailRepository.FindByOrderAndBookAsync(existingCart, existingBook);

            // Kiểm tra số lượng sách trong giỏ hàng không vượt quá số lượng tồn kho
            if (quantity > existingBook.Stock)
            {
                throw new Exception("Số lượng của sản phẩm trong giỏ hàng vượt quá mức cho phép");

            }
            else
            {

                // Nếu sách chưa có trong giỏ hàng
                if (bookInCart == null)
                {
                    bookInCart = new OrderDetail
                    {
                        Amount = quantity,
                        Book = existingBook,
                        Order = existingCart,
                        OriginalPrice = existingBook.Price,
                        SalePrice = existingBook.SalePrice
                    };
                    await _orderDetailRepository.AddAsync(bookInCart);
                }
                else
                {
                    // Cập nhật số lượng sách trong giỏ hàng
                    bookInCart.Amount += quantity;
                }
            }
            Console.WriteLine(existingBook.Stock);
            Console.WriteLine(bookInCart.Amount);

            // Lưu thay đổi vào cơ sở dữ liệu
            await _orderDetailRepository.SaveChangesAsync();
        }

        public async Task<List<Order>> GetAllCart()
        {
            return await _orderRepository.GetAllOrderAndState(OrderState.Cart);
        }

        public async Task<Order> GetCartByUser(long userId)
        {
            var existingUser = await GetExistingUserAsync(userId);
            var existingCart = await _orderRepository.GetByUserAndStateAsync(existingUser.Id, OrderState.Cart);

            if (existingCart == null)
            {
                // Creating a new cart if no existing cart is found
                existingCart = new Order()
                {
                    UserId = existingUser.Id,
                    FullName = existingUser.FullName,
                    Address = existingUser.Address,
                    Email = existingUser.Email,
                    Phone = existingUser.Phone,
                    ShippingPrice = 30000L, // Set default shipping price
                    ShippingState = ShippingState.NOTSHIPPING.ToString(), // Enum value
                    State = OrderState.Cart.ToString(), // Enum value
                    PaymentState = PaymentState.PENDING.ToString(), // Enum value
                    OrderDetails = new List<OrderDetail>() // Initialize as an empty list
                };

                await _orderRepository.AddNewCartAsync(existingCart);      // Save changes
            }

            return existingCart;
        }



        public async Task UpdateCart(Order order)
        {
            foreach (var orderDetail in order.OrderDetails)
            {
                var state = orderDetail.GetOrderDetailState();
                if (state == OrderDetailState.NOT_AVAILABLE)
                {
                    throw new Exception("Một số sản phẩm hiện không khả dụng do vượt quá số lượng mua");
                }
            }

            var existingOrder = await _orderRepository.GetOrderByOrderIdAsync(order.Id);
            if (existingOrder == null)
            {
                throw new Exception("Đã có lỗi xảy ra vui lòng tải lại trang");
            }

            await _orderRepository.SaveAsync(order);
        }
    }
}
