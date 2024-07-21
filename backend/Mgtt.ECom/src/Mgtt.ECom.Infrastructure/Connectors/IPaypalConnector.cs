namespace Mgtt.ECom.Infrastructure.Connectors;

public class OrderDetails
{
    public decimal TotalAmount { get; set; }
    public string CurrencyCode { get; set; }
    public string ReferenceId { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string AdminArea2 { get; set; }
    public string AdminArea1 { get; set; }
    public string PostalCode { get; set; }
    public string CountryCode { get; set; }
}

public class CreateOrderResponse
{
    public string OrderId { get; set; }
    public string CheckoutNowHref { get; set; }
}

public interface IPayPalConnector
{
    Task<string?> GetAccessTokenAsync();

    Task<CreateOrderResponse?> CreateOrderAsync(OrderDetails orderDetails);

    Task<string?> GetOrderAsync(string orderId);

    Task<bool> DeleteOrderAsync(string orderId);
}