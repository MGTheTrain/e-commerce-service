using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
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

    public async Task<CreateOrderResponse?> CreateOrderAsync(OrderDetails orderDetails)
    {
         try
        {
            var accessToken = await this.GetAccessTokenAsync();
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
                        reference_id = orderDetails.ReferenceId,
                        amount = new
                        {
                            currency_code = orderDetails.CurrencyCode,
                            value = orderDetails.TotalAmount.ToString("F2"),
                        },
                        shipping = new
                        {
                            address = new
                            {
                                address_line_1 = orderDetails.AddressLine1,
                                address_line_2 = orderDetails.AddressLine2,
                                admin_area_1 = orderDetails.AdminArea1,
                                admin_area_2 = orderDetails.AdminArea2,
                                postal_code = orderDetails.PostalCode,
                                country_code = orderDetails.CountryCode,
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

            var createOrderResponse = new CreateOrderResponse
            {
                OrderId = jsonDocument.RootElement.GetProperty("id").GetString()!,
                CheckoutNowHref = jsonDocument.RootElement
                .GetProperty("links")
                .EnumerateArray()
                .First(link => link.GetProperty("href").GetString().Contains("checkoutnow"))
                .GetProperty("href")
                .GetString()!,
            };

            this.logger.LogInformation("Order created successfully with Order ID: {OrderId}", createOrderResponse.OrderId);
            return createOrderResponse;
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

    public async Task<string?> GetOrderAsync(string orderId)
    {
        try
        {
            var accessToken = await this.GetAccessTokenAsync();
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

    public async Task<bool> DeleteOrderAsync(string orderId)
    {
        try
        {
            var accessToken = await this.GetAccessTokenAsync();
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