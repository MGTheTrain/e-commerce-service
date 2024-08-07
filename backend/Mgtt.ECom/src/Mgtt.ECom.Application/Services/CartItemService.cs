// <copyright file="CartItemService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Mgtt.ECom.Domain.ShoppingCart;
    using Mgtt.ECom.Persistence.DataAccess;
    using Microsoft.Extensions.Logging;

    public class CartItemService : ICartItemService
    {
        private readonly PsqlDbContext context;
        private readonly ILogger<CartItemService> logger;

        public CartItemService(PsqlDbContext context, ILogger<CartItemService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<CartItem?> GetCartItemById(Guid cartItemId)
        {
            this.logger.LogInformation("Fetching cart item by ID: {CartItemId}", cartItemId);
            try
            {
                return await Task.FromResult(this.context.CartItems.Find(cartItemId));
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error fetching cart item by ID: {CartItemId}", cartItemId);
                return await Task.FromResult<CartItem?>(null);
            }
        }

        public async Task<IEnumerable<CartItem>?> GetCartItemsByCartId(Guid cartId)
        {
            this.logger.LogInformation("Fetching cart items by CartID: {CartId}", cartId);
            try
            {
                return await Task.FromResult(this.context.CartItems.Where(ci => ci.CartID == cartId).ToList());
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error fetching cart items by CartID: {CartId}", cartId);
                return await Task.FromResult<IEnumerable<CartItem>?>(null);
            }
        }

        public async Task<CartItem?> GetCartItemByProductAndUserId(Guid productId, string userId)
        {
            this.logger.LogInformation("Fetching cart item by ProductID: {ProductId} and UserID: {UserId}", productId, userId);
            try
            {
                return await Task.FromResult(this.context.CartItems.Where(ci => ci.ProductID == productId && ci.UserID == userId).FirstOrDefault());
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error fetching cart item by ProductID: {ProductId} and UserID: {UserId}", productId, userId);
                return await Task.FromResult<CartItem?>(null);
            }
        }

        public async Task<CartItem?> CreateCartItem(CartItem cartItem)
        {
            this.logger.LogInformation("Creating cart item for CartID: {CartId}", cartItem.CartID);
            try
            {
                this.context.CartItems.Add(cartItem);
                await this.context.SaveChangesAsync();
                this.logger.LogInformation("Cart item created successfully for CartID: {CartId}", cartItem.CartID);
                return await Task.FromResult<CartItem?>(cartItem);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error creating cart item for CartID: {CartId}", cartItem.CartID);
                return await Task.FromResult<CartItem?>(null);
            }
        }

        public async Task<CartItem?> UpdateCartItem(CartItem cartItem)
        {
            this.logger.LogInformation("Updating cart item with ID: {CartItemId}", cartItem.CartItemID);
            try
            {
                this.context.CartItems.Update(cartItem);
                await this.context.SaveChangesAsync();
                this.logger.LogInformation("Cart item updated successfully with ID: {CartItemId}", cartItem.CartItemID);
                return await Task.FromResult<CartItem?>(cartItem);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error updating cart item with ID: {CartItemId}", cartItem.CartItemID);
                return await Task.FromResult<CartItem?>(null);
            }
        }

        public async Task DeleteCartItem(Guid cartItemId)
        {
            this.logger.LogInformation("Deleting cart item with ID: {CartItemId}", cartItemId);
            try
            {
                var cartItem = await this.context.CartItems.FindAsync(cartItemId);
                if (cartItem != null)
                {
                    this.context.CartItems.Remove(cartItem);
                    await this.context.SaveChangesAsync();
                    this.logger.LogInformation("Cart item deleted successfully with ID: {CartItemId}", cartItemId);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error deleting cart item with ID: {CartItemId}", cartItemId);
            }
        }
    }
}
