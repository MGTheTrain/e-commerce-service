namespace Mgtt.ECom.Domain.ShoppingCart;

public interface ICartService
{
    Task<Cart> GetCartByUserId(Guid userId);
    Task CreateCart(Cart cart);
    Task UpdateCart(Cart cart);
    Task DeleteCart(Guid cartId);
}
