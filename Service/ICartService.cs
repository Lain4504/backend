using BackEnd.Models;

public interface ICartService
{
    Task AddToCart(int bookId, decimal price, int quantity, int userId);
    Task<Order> GetCartByUser(long userId);

    Task UpdateCart(Order order);

    Task<List<Order>> GetAllCart();
}
