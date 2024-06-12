namespace Mgtt.ECom.Domain.OrderManagement;

public interface IOrderService
{
    Task<Order> GetOrderById(Guid orderId);
    Task<IEnumerable<Order>> GetOrdersByUserId(Guid userId);
    Task CreateOrder(Order order);
    Task UpdateOrder(Order order);
    Task DeleteOrder(Guid orderId);
}
