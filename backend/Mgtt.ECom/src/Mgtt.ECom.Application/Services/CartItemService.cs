using Mgtt.ECom.Domain.ShoppingCart;
using Mgtt.ECom.Persistence.DataAccess;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mgtt.ECom.Application.Services
{
    public class CartItemService : ICartItemService
    {
        private readonly PsqlDbContext _context;
        private readonly ILogger<CartItemService> _logger;

        public CartItemService(PsqlDbContext context, ILogger<CartItemService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<CartItem?> GetCartItemById(Guid cartItemId)
        {
            _logger.LogInformation("Fetching cart item by ID: {CartItemId}", cartItemId);
            try
            {
                return await Task.FromResult(_context.CartItems.Find(cartItemId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching cart item by ID: {CartItemId}", cartItemId);
                return await Task.FromResult<CartItem?>(null);
            }
        }

        public async Task<IEnumerable<CartItem>?> GetCartItemsByCartId(Guid cartId)
        {
            _logger.LogInformation("Fetching cart items by CartID: {CartId}", cartId);
            try
            {
                return await Task.FromResult(_context.CartItems.Where(ci => ci.CartID == cartId).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching cart items by CartID: {CartId}", cartId);
                return await Task.FromResult<IEnumerable<CartItem>?>(null);
            }
        }

        public async Task<CartItem?> CreateCartItem(CartItem cartItem)
        {
            _logger.LogInformation("Creating cart item for CartID: {CartId}", cartItem.CartID);
            try
            {
                _context.CartItems.Add(cartItem);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Cart item created successfully for CartID: {CartId}", cartItem.CartID);
                return await Task.FromResult<CartItem?>(cartItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating cart item for CartID: {CartId}", cartItem.CartID);
                return await Task.FromResult<CartItem?>(null);
            }
        }

        public async Task<CartItem?> UpdateCartItem(CartItem cartItem)
        {
            _logger.LogInformation("Updating cart item with ID: {CartItemId}", cartItem.CartItemID);
            try
            {
                _context.CartItems.Update(cartItem);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Cart item updated successfully with ID: {CartItemId}", cartItem.CartItemID);
                return await Task.FromResult<CartItem?>(cartItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating cart item with ID: {CartItemId}", cartItem.CartItemID);
                return await Task.FromResult<CartItem?>(null);
            }
        }

        public async Task DeleteCartItem(Guid cartItemId)
        {
            _logger.LogInformation("Deleting cart item with ID: {CartItemId}", cartItemId);
            try
            {
                var cartItem = await _context.CartItems.FindAsync(cartItemId);
                if (cartItem != null)
                {
                    _context.CartItems.Remove(cartItem);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Cart item deleted successfully with ID: {CartItemId}", cartItemId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting cart item with ID: {CartItemId}", cartItemId);
            }
        }
    }
}
