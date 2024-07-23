using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Mgtt.ECom.Domain.OrderManagement;
using Mgtt.ECom.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Mgtt.ECom.Infrastructure.Connectors;
public class PayPalConnector : IPayPalConnector
{
    private readonly HttpClient httpClient;
    private readonly PayPalSettings settings;
    private readonly ILogger<PayPalConnector> logger;

    public PayPalConnector(HttpClient httpClient, IOptions<PayPalSettings> settings, ILogger<PayPalConnector> logger)
    {
        this.httpClient = httpClient;
        this.settings = settings.Value;
        this.logger = logger;
    }

    public async Task<string?> GetAccessTokenAsync()
    {
        try
        {
            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{this.settings.ClientId}:{this.settings.ClientSecret}"));

            var request = new HttpRequestMessage(HttpMethod.Post, $"{this.settings.BaseUrl}/v1/oauth2/token")
            {
                Headers =
                {
                    Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials),
                },
                Content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials"),
                }),
            };

            var response = await this.httpClient.SendAsync(request);

            if (response == null)
            {
                throw new InvalidOperationException("The response from the HTTP request is null.");
            }

            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(jsonString);

            this.logger.LogInformation("Access token retrieved successfully.");
            return result?.access_token;
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "An unexpected error occurred while retrieving the access token.");
            return null;
        }
    }

    public async Task<Order?> CreateOrderAsync(Order order, string accessToken)
    {
         try
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                this.logger.LogWarning("Failed to retrieve access token.");
                return null;
            }

            var requestBody = new
            {
                intent = "CAPTURE",
                purchase_units = new[]
                {
                    new
                    {
                        reference_id = order.ReferenceId,
                        amount = new
                        {
                            currency_code = order.CurrencyCode,
                            value = order.TotalAmount.ToString("F2"),
                        },
                        shipping = new
                        {
                            address = new
                            {
                                address_line_1 = order.AddressLine1,
                                address_line_2 = order.AddressLine2,
                                admin_area_1 = order.AdminArea1,
                                admin_area_2 = order.AdminArea2,
                                postal_code = order.PostalCode,
                                country_code = order.CountryCode,
                            },
                        },
                    },
                },
            };

            var requestContent = new StringContent(JsonConvert.SerializeObject(requestBody), System.Text.Encoding.UTF8, "application/json");
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{this.settings.BaseUrl}/v2/checkout/orders")
            {
                Content = requestContent,
            };

            requestMessage.Headers.Add("Authorization", $"Bearer {accessToken}");

            var response = await this.httpClient.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var jsonDocument = JsonDocument.Parse(responseContent);

            order.OrderID = jsonDocument.RootElement.GetProperty("id").GetString()!;
            order.CheckoutNowHref = jsonDocument.RootElement
                .GetProperty("links")
                .EnumerateArray()
                .First(link => link.GetProperty("href").GetString()!.Contains("checkoutnow"))
                .GetProperty("href")
                .GetString()!;

            this.logger.LogInformation("Order created successfully with Order ID: {OrderId}", order.OrderID);
            return order;
        }
        catch (HttpRequestException ex)
        {
            this.logger.LogError(ex, "HTTP request failed while creating order.");
            return null;
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "An unexpected error occurred while creating order.");
            return null;
        }
    }

    public async Task<string?> GetOrderByIdAsync(string orderId, string accessToken)
    {
        try
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                this.logger.LogWarning("Failed to retrieve access token.");
                return null;
            }

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{this.settings.BaseUrl}/v2/checkout/orders/{orderId}");
            requestMessage.Headers.Add("Authorization", $"Bearer {accessToken}");

            var response = await this.httpClient.SendAsync(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                this.logger.LogWarning("Failed to retrieve order. Status Code: {StatusCode}", response.StatusCode);
                return null;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            this.logger.LogInformation("Order retrieved successfully. Order ID: {OrderId}", orderId);
            return responseContent;
        }
        catch (HttpRequestException ex)
        {
            this.logger.LogError(ex, "HTTP request failed while retrieving order.");
            return null;
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "An unexpected error occurred while retrieving order.");
            return null;
        }
    }

    public async Task<Order?> UpdateOrderByIdAsync(string orderId, Order order, string accessToken) 
    {
        try
        {
            var result = await this.DeleteOrderByIdAsync(orderId, accessToken);
            if(result)
            {
                var newOrder = await this.CreateOrderAsync(order, accessToken);
                this.logger.LogInformation("Order updated successfully. Order ID: {OrderId}", newOrder.OrderID);
                return newOrder;
            }
            else {
                return null;
            }
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "An unexpected error occurred while updating order.");
            return null;
        }
    }

    public async Task<bool> DeleteOrderByIdAsync(string orderId, string accessToken)
    {
        try
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                this.logger.LogWarning("Failed to retrieve access token.");
                return false;
            }

            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"{this.settings.BaseUrl}/v1/checkout/orders/{orderId}");
            requestMessage.Headers.Add("Authorization", $"Bearer {accessToken}");

            var response = await this.httpClient.SendAsync(requestMessage);
            if (!response.IsSuccessStatusCode)
            {
                this.logger.LogWarning("Failed to delete order. Status Code: {StatusCode}", response.StatusCode);
                return false;
            }

            this.logger.LogInformation("Order deleted successfully. Order ID: {OrderId}", orderId);
            return true;
        }
        catch (HttpRequestException ex)
        {
            this.logger.LogError(ex, "HTTP request failed while deleting order.");
            return false;
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "An unexpected error occurred while deleting order.");
            return false;
        }
    }
}
