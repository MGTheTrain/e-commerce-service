using Mgtt.ECom.Domain.ShoppingCart;
using Mgtt.ECom.Persistence.DataAccess;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mgtt.ECom.Application.Services
{
    public class CartItemService : ICartItemService
    {
        private readonly PsqlDbContext _context;

        public CartItemService(PsqlDbContext context)
        {
            _context = context;
        }

        public async Task<CartItem> GetCartItemById(Guid cartItemId)
        {
            return await Task.FromResult(_context.CartItems.Find(cartItemId));
        }

        public async Task<IEnumerable<CartItem>> GetCartItemsByCartId(Guid cartId)
        {
            return await Task.FromResult(_context.CartItems.Where(ci => ci.CartID == cartId).ToList());
        }

        public async Task CreateCartItem(CartItem cartItem)
        {
            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCartItem(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCartItem(Guid cartItemId)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }
    }
}
