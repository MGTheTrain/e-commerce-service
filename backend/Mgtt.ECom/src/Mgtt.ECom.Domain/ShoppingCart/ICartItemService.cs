namespace Mgtt.ECom.Domain.ShoppingCart;

public interface ICartItemService
{
    Task<CartItem> GetCartItemById(int cartItemId);
    Task<IEnumerable<CartItem>> GetCartItemsByCartId(int cartId);
    Task CreateCartItem(CartItem cartItem);
    Task UpdateCartItem(CartItem cartItem);
    Task DeleteCartItem(int cartItemId);
}
