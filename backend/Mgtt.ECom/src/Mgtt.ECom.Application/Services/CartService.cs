using Mgtt.ECom.Domain.ShoppingCart;
using Mgtt.ECom.Persistence.DataAccess;
using System.Linq;
using System.Threading.Tasks;

namespace Mgtt.ECom.Application.Services
{
    public class CartService : ICartService
    {
        private readonly PsqlDbContext _context;

        public CartService(PsqlDbContext context)
        {
            _context = context;
        }

        public async Task<Cart> GetCartByUserId(int userId)
        {
            return await Task.FromResult(_context.Carts.SingleOrDefault(c => c.UserID == userId));
        }

        public async Task CreateCart(Cart cart)
        {
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCart(Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCart(int cartId)
        {
            var cart = await _context.Carts.FindAsync(cartId);
            if (cart != null)
            {
                _context.Carts.Remove(cart);
                await _context.SaveChangesAsync();
            }
        }
    }
}
