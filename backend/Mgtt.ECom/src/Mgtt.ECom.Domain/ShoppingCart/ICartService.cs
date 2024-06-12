namespace Mgtt.ECom.Domain.ShoppingCart;

public interface ICartService
{
    Cart GetCartByUserId(int userId);
    void CreateCart(Cart cart);
    void UpdateCart(Cart cart);
    void DeleteCart(int cartId);
}
