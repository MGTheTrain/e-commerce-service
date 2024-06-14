namespace Mgtt.ECom.Domain.OrderManagement;

public interface IOrderService
{
    Task<Order?> GetOrderById(Guid orderId);
    Task<IEnumerable<Order>?> GetOrdersByUserId(Guid userId);
    Task<Order?> CreateOrder(Order order);
    Task<Order?> UpdateOrder(Order order);
    Task DeleteOrder(Guid orderId);
}
