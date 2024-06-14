using Mgtt.ECom.Domain.OrderManagement;
using Mgtt.ECom.Persistence.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mgtt.ECom.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly PsqlDbContext _context;
        private readonly ILogger<OrderService> _logger;

        public OrderService(PsqlDbContext context, ILogger<OrderService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Order?> GetOrderById(Guid orderId)
        {
            _logger.LogInformation("Fetching order by ID: {OrderId}", orderId);
            try
            {
                return await _context.Orders.FindAsync(orderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching order by ID: {OrderId}", orderId);
                return await Task.FromResult<Order?>(null);
            }
        }

        public async Task<IEnumerable<Order>?> GetOrdersByUserId(Guid userId)
        {
            _logger.LogInformation("Fetching orders by User ID: {UserId}", userId);
            try
            {
                return await _context.Orders.Where(o => o.UserID == userId).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching orders by User ID: {UserId}", userId);
                return await Task.FromResult<IEnumerable<Order>?>(null);
            }
        }

        public async Task<Order?> CreateOrder(Order order)
        {
            _logger.LogInformation("Creating new order: {OrderId}", order.OrderID);
            try
            {
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Order created successfully: {OrderId}", order.OrderID);
                return await Task.FromResult<Order?>(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order: {OrderId}", order.OrderID);
                return await Task.FromResult<Order?>(null);
            }
        }

        public async Task<Order?> UpdateOrder(Order order)
        {
            _logger.LogInformation("Updating order: {OrderId}", order.OrderID);
            try
            {
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Order updated successfully: {OrderId}", order.OrderID);
                return await Task.FromResult<Order?>(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating order: {OrderId}", order.OrderID);
                return await Task.FromResult<Order?>(null);
            }
        }

        public async Task DeleteOrder(Guid orderId)
        {
            _logger.LogInformation("Deleting order: {OrderId}", orderId);
            try
            {
                var order = await _context.Orders.FindAsync(orderId);
                if (order != null)
                {
                    _context.Orders.Remove(order);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Order deleted successfully: {OrderId}", orderId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting order: {OrderId}", orderId);
            }
        }
    }
}
