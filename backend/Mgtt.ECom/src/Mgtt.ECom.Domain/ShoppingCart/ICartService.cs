namespace Mgtt.ECom.Domain.ShoppingCart;

public interface ICartService
{
    Task<Cart> GetCartByUserId(int userId);
    Task CreateCart(Cart cart);
    Task UpdateCart(Cart cart);
    Task DeleteCart(int cartId);
}
