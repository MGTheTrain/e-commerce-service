namespace Mgtt.ECom.Domain.ShoppingCart;

public interface ICartService
{
    Task<Cart?> GetCartById(Guid cartId);
    Task<Cart?> CreateCart(Cart cart);
    Task<Cart?> UpdateCart(Cart cart);
    Task DeleteCart(Guid cartId);
}
