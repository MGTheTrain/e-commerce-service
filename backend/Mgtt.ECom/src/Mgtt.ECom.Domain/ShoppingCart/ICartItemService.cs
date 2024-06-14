namespace Mgtt.ECom.Domain.ShoppingCart;

public interface ICartItemService
{
    Task<CartItem?> GetCartItemById(Guid cartItemId);
    Task<IEnumerable<CartItem>?> GetCartItemsByCartId(Guid cartId);
    Task<CartItem?> CreateCartItem(CartItem cartItem);
    Task<CartItem?> UpdateCartItem(CartItem cartItem);
    Task DeleteCartItem(Guid cartItemId);
}
