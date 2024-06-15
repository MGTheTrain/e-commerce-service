using Mgtt.ECom.Domain.ShoppingCart;
using Mgtt.ECom.Persistence.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mgtt.ECom.Application.Services
{
    public class CartService : ICartService
    {
        private readonly PsqlDbContext _context;
        private readonly ILogger<CartService> _logger;

        public CartService(PsqlDbContext context, ILogger<CartService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Cart?> GetCartById(Guid cartId)
        {
            _logger.LogInformation("Fetching cart by CartID: {cartId}", cartId);
            try
            {
                return await Task.FromResult(_context.Carts.Find(cartId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching cart by CartID: {cartId}", cartId);
                return await Task.FromResult<Cart?>(null);
            }
        }

        public async Task<Cart?> CreateCart(Cart cart)
        {
            _logger.LogInformation("Creating new cart for UserID: {UserId}", cart.UserID);
            try
            {
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Cart created successfully for UserID: {UserId}", cart.UserID);
                return await Task.FromResult<Cart?>(cart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating cart for UserID: {UserId}", cart.UserID);
                return await Task.FromResult<Cart?>(null);
            }
        }

        public async Task<Cart?> UpdateCart(Cart cart)
        {
            _logger.LogInformation("Updating cart for UserID: {UserId}", cart.UserID);
            try
            {
                _context.Carts.Update(cart);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Cart updated successfully for UserID: {UserId}", cart.UserID);
                return await Task.FromResult<Cart?>(cart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating cart for UserID: {UserId}", cart.UserID);
                return await Task.FromResult<Cart?>(null);
            }
        }

        public async Task DeleteCart(Guid cartId)
        {
            _logger.LogInformation("Deleting cart with ID: {CartId}", cartId);
            try
            {
                var cart = await _context.Carts.FindAsync(cartId);
                if (cart != null)
                {
                    _context.Carts.Remove(cart);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Cart deleted successfully with ID: {CartId}", cartId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting cart with ID: {CartId}", cartId);
            }
        }
    }
}
