namespace Mgtt.ECom.Domain.OrderManagement;

public interface IOrderService
{
    Task<Order> GetOrderById(int orderId);
    Task<IEnumerable<Order>> GetOrdersByUserId(int userId);
    Task CreateOrder(Order order);
    Task UpdateOrder(Order order);
    Task DeleteOrder(int orderId);
}
