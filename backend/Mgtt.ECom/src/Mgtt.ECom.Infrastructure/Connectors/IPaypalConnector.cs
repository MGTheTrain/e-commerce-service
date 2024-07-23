using Mgtt.ECom.Domain.OrderManagement;

namespace Mgtt.ECom.Infrastructure.Connectors;

public interface IPayPalConnector
{
    Task<string?> GetAccessTokenAsync();

    Task<Order?> CreateOrderAsync(Order order, string accessToken);

    Task<string?> GetOrderByIdAsync(string orderId, string accessToken);

    Task<Order?> UpdateOrderByIdAsync(string orderId, Order order, string accessToken); // Delete and create new order

    Task<bool> DeleteOrderByIdAsync(string orderId, string accessToken);
}