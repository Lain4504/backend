using BackEnd.Models;

public interface ICartService
{
    Task AddToCart(OrderDetail orderDetail);

    Task<Order> GetCartByUser(long userId);

    Task UpdateCart(Order order);

    Task<List<Order>> GetAllCart();
}
