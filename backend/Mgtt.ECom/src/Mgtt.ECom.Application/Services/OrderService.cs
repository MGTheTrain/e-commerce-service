// <copyright file="OrderService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Mgtt.ECom.Domain.OrderManagement;
    using Mgtt.ECom.Infrastructure.Connectors;
    using Mgtt.ECom.Persistence.DataAccess;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class OrderService : IOrderService
    {
        private readonly PsqlDbContext context;
        private readonly ILogger<OrderService> logger;
        private readonly IPayPalConnector payPalConnector; 

        public OrderService(PsqlDbContext context, IPayPalConnector payPalConnector, ILogger<OrderService> logger)
        {
            this.context = context;
            this.logger = logger;
            this.payPalConnector = payPalConnector;
        }

        public async Task<Order?> GetOrderById(string orderId)
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

        public async Task<IEnumerable<Order>?> GetOrdersByUserId(string userId)
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
                var accessToken = await this.payPalConnector.GetAccessTokenAsync();
                var updatedOrder = await this.payPalConnector.CreateOrderAsync(order, accessToken!);

                this.context.Orders.Add(updatedOrder!);
                await this.context.SaveChangesAsync();
                this.logger.LogInformation("Order created successfully: {OrderId}", updatedOrder!.OrderID);
                return await Task.FromResult<Order?>(updatedOrder);
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
                var accessToken = await this.payPalConnector.GetAccessTokenAsync();
                var updatedOrder = await this.payPalConnector.UpdateOrderByIdAsync(order.OrderID, order, accessToken!);

                this.context.Orders.Update(updatedOrder!);
                await this.context.SaveChangesAsync();
                this.logger.LogInformation("Order updated successfully: {OrderId}", updatedOrder!.OrderID);
                return await Task.FromResult<Order?>(updatedOrder);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error updating order: {OrderId}", order.OrderID);
                return await Task.FromResult<Order?>(null);
            }
        }

        public async Task DeleteOrder(string orderId)
        {
            this.logger.LogInformation("Deleting order: {OrderId}", orderId);
            try
            {
                var accessToken = await this.payPalConnector.GetAccessTokenAsync();
                var result = await this.payPalConnector.DeleteOrderByIdAsync(orderId, accessToken!);

                if (result)
                {
                    var order = await this.context.Orders.FindAsync(orderId);
                    if (order != null)
                    {
                        this.context.Orders.Remove(order);
                        await this.context.SaveChangesAsync();
                        this.logger.LogInformation("Order deleted successfully: {OrderId}", orderId);
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error deleting order: {OrderId}", orderId);
            }
        }
    }
}
