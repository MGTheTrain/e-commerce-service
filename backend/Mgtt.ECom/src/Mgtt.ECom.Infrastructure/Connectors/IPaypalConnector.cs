using Mgtt.ECom.Domain.OrderManagement;

namespace Mgtt.ECom.Infrastructure.Connectors;

public interface IPayPalConnector
{
    Task<string?> GetAccessTokenAsync();

    Task<Order?> CreateOrderAsync(Order order, string accessToken);

    Task<string?> GetOrderByIdAsync(string orderId, string accessToken);

    Task<bool> DeleteOrderByIdAsync(string orderId, string accessToken);
}