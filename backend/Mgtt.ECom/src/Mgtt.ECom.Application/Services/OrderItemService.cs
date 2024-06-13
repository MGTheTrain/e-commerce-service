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
    public class OrderItemService : IOrderItemService
    {
        private readonly PsqlDbContext _context;
        private readonly ILogger<OrderItemService> _logger;

        public OrderItemService(PsqlDbContext context, ILogger<OrderItemService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<OrderItem> GetOrderItemById(Guid orderItemId)
        {
            _logger.LogInformation("Fetching order item by ID: {OrderItemId}", orderItemId);
            try
            {
                return await _context.OrderItems.FindAsync(orderItemId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching order item by ID: {OrderItemId}", orderItemId);
                throw;
            }
        }

        public async Task<IEnumerable<OrderItem>> GetOrderItemsByOrderId(Guid orderId)
        {
            _logger.LogInformation("Fetching order items by Order ID: {OrderId}", orderId);
            try
            {
                return await _context.OrderItems.Where(oi => oi.OrderID == orderId).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching order items by Order ID: {OrderId}", orderId);
                throw;
            }
        }

        public async Task CreateOrderItem(OrderItem orderItem)
        {
            _logger.LogInformation("Creating new order item: {OrderItemId}", orderItem.OrderItemID);
            try
            {
                _context.OrderItems.Add(orderItem);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Order item created successfully: {OrderItemId}", orderItem.OrderItemID);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order item: {OrderItemId}", orderItem.OrderItemID);
                throw;
            }
        }

        public async Task UpdateOrderItem(OrderItem orderItem)
        {
            _logger.LogInformation("Updating order item: {OrderItemId}", orderItem.OrderItemID);
            try
            {
                _context.OrderItems.Update(orderItem);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Order item updated successfully: {OrderItemId}", orderItem.OrderItemID);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating order item: {OrderItemId}", orderItem.OrderItemID);
                throw;
            }
        }

        public async Task DeleteOrderItem(Guid orderItemId)
        {
            _logger.LogInformation("Deleting order item: {OrderItemId}", orderItemId);
            try
            {
                var orderItem = await _context.OrderItems.FindAsync(orderItemId);
                if (orderItem != null)
                {
                    _context.OrderItems.Remove(orderItem);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Order item deleted successfully: {OrderItemId}", orderItemId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting order item: {OrderItemId}", orderItemId);
                throw;
            }
        }
    }
}
