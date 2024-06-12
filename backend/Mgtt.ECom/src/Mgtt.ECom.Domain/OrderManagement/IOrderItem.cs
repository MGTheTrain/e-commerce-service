namespace Mgtt.ECom.Domain.OrderManagement;

public interface IOrderItemService
{
    Task<OrderItem> GetOrderItemById(int orderItemId);
    Task<IEnumerable<OrderItem>> GetOrderItemsByOrderId(int orderId);
    Task CreateOrderItem(OrderItem orderItem);
    Task UpdateOrderItem(OrderItem orderItem);
    Task DeleteOrderItem(int orderItemId);
}
