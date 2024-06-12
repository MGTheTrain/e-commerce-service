namespace Mgtt.EComm.Domain.ShoppingCart;

public interface ICartItemService
{
    CartItem GetCartItemById(int cartItemId);
    IEnumerable<CartItem> GetCartItemsByCartId(int cartId);
    void CreateCartItem(CartItem cartItem);
    void UpdateCartItem(CartItem cartItem);
    void DeleteCartItem(int cartItemId);
}
