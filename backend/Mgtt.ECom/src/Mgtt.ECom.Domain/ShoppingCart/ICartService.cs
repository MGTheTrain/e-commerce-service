namespace Mgtt.ECom.Domain.ShoppingCart;

public interface ICartService
{
    Task<Cart?> GetCartByUserId(Guid userId);
    Task<Cart?> CreateCart(Cart cart);
    Task<Cart?> UpdateCart(Cart cart);
    Task DeleteCart(Guid cartId);
}
