// <copyright file="OrderService.cs" company="PlaceholderCompany">
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

    public class OrderService : IOrderService
    {
        private readonly PsqlDbContext context;
        private readonly ILogger<OrderService> logger;

        public OrderService(PsqlDbContext context, ILogger<OrderService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<Order?> GetOrderById(Guid orderId)
        {
            this.logger.LogInformation("Fetching order by ID: {OrderId}", orderId);
            try
            {
                return await this.context.Orders.FindAsync(orderId);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error fetching order by ID: {OrderId}", orderId);
                return await Task.FromResult<Order?>(null);
            }
        }

        public async Task<IEnumerable<Order>?> GetOrdersByUserId(Guid userId)
        {
            this.logger.LogInformation("Fetching orders by User ID: {UserId}", userId);
            try
            {
                return await this.context.Orders.Where(o => o.UserID == userId).ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error fetching orders by User ID: {UserId}", userId);
                return await Task.FromResult<IEnumerable<Order>?>(null);
            }
        }

        public async Task<Order?> CreateOrder(Order order)
        {
            this.logger.LogInformation("Creating new order: {OrderId}", order.OrderID);
            try
            {
                this.context.Orders.Add(order);
                await this.context.SaveChangesAsync();
                this.logger.LogInformation("Order created successfully: {OrderId}", order.OrderID);
                return await Task.FromResult<Order?>(order);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error creating order: {OrderId}", order.OrderID);
                return await Task.FromResult<Order?>(null);
            }
        }

        public async Task<Order?> UpdateOrder(Order order)
        {
            this.logger.LogInformation("Updating order: {OrderId}", order.OrderID);
            try
            {
                this.context.Orders.Update(order);
                await this.context.SaveChangesAsync();
                this.logger.LogInformation("Order updated successfully: {OrderId}", order.OrderID);
                return await Task.FromResult<Order?>(order);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error updating order: {OrderId}", order.OrderID);
                return await Task.FromResult<Order?>(null);
            }
        }

        public async Task DeleteOrder(Guid orderId)
        {
            this.logger.LogInformation("Deleting order: {OrderId}", orderId);
            try
            {
                var order = await this.context.Orders.FindAsync(orderId);
                if (order != null)
                {
                    this.context.Orders.Remove(order);
                    await this.context.SaveChangesAsync();
                    this.logger.LogInformation("Order deleted successfully: {OrderId}", orderId);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error deleting order: {OrderId}", orderId);
            }
        }
    }
}
