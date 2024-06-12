using Mgtt.ECom.Domain.OrderManagement;
using Mgtt.ECom.Persistence.DataAccess;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mgtt.ECom.Application.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly PsqlDbContext _context;

        public OrderItemService(PsqlDbContext context)
        {
            _context = context;
        }

        public async Task<OrderItem> GetOrderItemById(int orderItemId)
        {
            return await Task.FromResult(_context.OrderItems.Find(orderItemId));
        }

        public async Task<IEnumerable<OrderItem>> GetOrderItemsByOrderId(int orderId)
        {
            return await Task.FromResult(_context.OrderItems.Where(oi => oi.OrderID == orderId).ToList());
        }

        public async Task CreateOrderItem(OrderItem orderItem)
        {
            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderItem(OrderItem orderItem)
        {
            _context.OrderItems.Update(orderItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderItem(int orderItemId)
        {
            var orderItem = await _context.OrderItems.FindAsync(orderItemId);
            if (orderItem != null)
            {
                _context.OrderItems.Remove(orderItem);
                await _context.SaveChangesAsync();
            }
        }
    }
}
