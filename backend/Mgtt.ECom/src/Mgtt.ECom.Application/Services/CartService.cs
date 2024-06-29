// <copyright file="CartService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Application.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Mgtt.ECom.Domain.ShoppingCart;
    using Mgtt.ECom.Persistence.DataAccess;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class CartService : ICartService
    {
        private readonly PsqlDbContext context;
        private readonly ILogger<CartService> logger;

        public CartService(PsqlDbContext context, ILogger<CartService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<Cart?> GetCartById(Guid cartId)
        {
            this.logger.LogInformation("Fetching cart by CartID: {cartId}", cartId);
            try
            {
                return await Task.FromResult(this.context.Carts.Find(cartId));
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error fetching cart by CartID: {cartId}", cartId);
                return await Task.FromResult<Cart?>(null);
            }
        }

        public async Task<IEnumerable<Cart>?> GetAllCarts()
        {
            this.logger.LogInformation("Fetching all carts");
            try
            {
                return await this.context.Carts.ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error fetching all carts");
                return await Task.FromResult<IEnumerable<Cart>?>(null);
            }
        }

        public async Task<Cart?> CreateCart(Cart cart)
        {
            this.logger.LogInformation("Creating new cart for UserID: {UserId}", cart.UserID);
            try
            {
                this.context.Carts.Add(cart);
                await this.context.SaveChangesAsync();
                this.logger.LogInformation("Cart created successfully for UserID: {UserId}", cart.UserID);
                return await Task.FromResult<Cart?>(cart);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error creating cart for UserID: {UserId}", cart.UserID);
                return await Task.FromResult<Cart?>(null);
            }
        }

        public async Task<Cart?> UpdateCart(Cart cart)
        {
            this.logger.LogInformation("Updating cart for UserID: {UserId}", cart.UserID);
            try
            {
                this.context.Carts.Update(cart);
                await this.context.SaveChangesAsync();
                this.logger.LogInformation("Cart updated successfully for UserID: {UserId}", cart.UserID);
                return await Task.FromResult<Cart?>(cart);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error updating cart for UserID: {UserId}", cart.UserID);
                return await Task.FromResult<Cart?>(null);
            }
        }

        public async Task DeleteCart(Guid cartId)
        {
            this.logger.LogInformation("Deleting cart with ID: {CartId}", cartId);
            try
            {
                var cart = await this.context.Carts.FindAsync(cartId);
                if (cart != null)
                {
                    this.context.Carts.Remove(cart);
                    await this.context.SaveChangesAsync();
                    this.logger.LogInformation("Cart deleted successfully with ID: {CartId}", cartId);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error deleting cart with ID: {CartId}", cartId);
            }
        }
    }
}
