using Mgtt.ECom.Domain.OrderManagement;
using Mgtt.ECom.Persistence.DataAccess;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mgtt.ECom.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly PsqlDbContext _context;

        public OrderService(PsqlDbContext context)
        {
            _context = context;
        }

        public async Task<Order> GetOrderById(Guid orderId)
        {
            return await Task.FromResult(_context.Orders.Find(orderId));
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserId(Guid userId)
        {
            return await Task.FromResult(_context.Orders.Where(o => o.UserID == userId).ToList());
        }

        public async Task CreateOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrder(Guid orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
    }
}
