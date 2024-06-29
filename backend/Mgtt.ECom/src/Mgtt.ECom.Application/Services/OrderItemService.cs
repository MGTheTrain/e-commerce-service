// <copyright file="OrderItemService.cs" company="PlaceholderCompany">
// Copyright (c) MGTheTrain. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Mgtt.ECom.Domain.OrderManagement;
    using Mgtt.ECom.Persistence.DataAccess;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class OrderItemService : IOrderItemService
    {
        private readonly PsqlDbContext context;
        private readonly ILogger<OrderItemService> logger;

        public OrderItemService(PsqlDbContext context, ILogger<OrderItemService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<OrderItem?> GetOrderItemById(Guid orderItemId)
        {
            this.logger.LogInformation("Fetching order item by ID: {OrderItemId}", orderItemId);
            try
            {
                var orderItem = await this.context.OrderItems.FindAsync(orderItemId);
                return await Task.FromResult<OrderItem?>(orderItem);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error fetching order item by ID: {OrderItemId}", orderItemId);
                return await Task.FromResult<OrderItem?>(null);
            }
        }

        public async Task<IEnumerable<OrderItem>?> GetOrderItemsByOrderId(Guid orderId)
        {
            this.logger.LogInformation("Fetching order items by Order ID: {OrderId}", orderId);
            try
            {
                return await this.context.OrderItems.Where(oi => oi.OrderID == orderId).ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error fetching order items by Order ID: {OrderId}", orderId);
                return await Task.FromResult<IEnumerable<OrderItem>?>(null);
            }
        }

        public async Task<OrderItem?> CreateOrderItem(OrderItem orderItem)
        {
            this.logger.LogInformation("Creating new order item: {OrderItemId}", orderItem.OrderItemID);
            try
            {
                this.context.OrderItems.Add(orderItem);
                await this.context.SaveChangesAsync();
                this.logger.LogInformation("Order item created successfully: {OrderItemId}", orderItem.OrderItemID);
                return await Task.FromResult<OrderItem?>(orderItem);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error creating order item: {OrderItemId}", orderItem.OrderItemID);
                return await Task.FromResult<OrderItem?>(null);
            }
        }

        public async Task<OrderItem?> UpdateOrderItem(OrderItem orderItem)
        {
            this.logger.LogInformation("Updating order item: {OrderItemId}", orderItem.OrderItemID);
            try
            {
                this.context.OrderItems.Update(orderItem);
                await this.context.SaveChangesAsync();
                this.logger.LogInformation("Order item updated successfully: {OrderItemId}", orderItem.OrderItemID);
                return await Task.FromResult<OrderItem?>(orderItem);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error updating order item: {OrderItemId}", orderItem.OrderItemID);
                return await Task.FromResult<OrderItem?>(null);
            }
        }

        public async Task DeleteOrderItem(Guid orderItemId)
        {
            this.logger.LogInformation("Deleting order item: {OrderItemId}", orderItemId);
            try
            {
                var orderItem = await this.context.OrderItems.FindAsync(orderItemId);
                if (orderItem != null)
                {
                    this.context.OrderItems.Remove(orderItem);
                    await this.context.SaveChangesAsync();
                    this.logger.LogInformation("Order item deleted successfully: {OrderItemId}", orderItemId);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error deleting order item: {OrderItemId}", orderItemId);
            }
        }
    }
}
