using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Mgtt.ECom.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Mgtt.ECom.Infrastructure.Connectors;
public class PayPalConnector : IPayPalConnector
{
    private readonly HttpClient _httpClient;
    private readonly PayPalSettings _settings;

    public PayPalConnector(HttpClient httpClient, IOptions<PayPalSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
    }

    public async Task<string> GetAccessTokenAsync()
    {
        var basicAuth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_settings.ClientId}:{_settings.ClientSecret}"));
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", basicAuth);

        var request = new HttpRequestMessage(HttpMethod.Post, "https://api-m.sandbox.paypal.com/v1/oauth2/token");
        request.Headers.Add("Accept", "application/json");
        request.Headers.Add("Accept-Language", "en_US");
        request.Content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var jsonString = await response.Content.ReadAsStringAsync();
        dynamic result = JsonConvert.DeserializeObject(jsonString)!;
        return result.access_token;
    }

    public async Task<CreateOrderResponse> CreateOrderAsync(OrderDetails orderDetails)
    {
        var accessToken = await GetAccessTokenAsync();

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

        var requestContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://api-m.sandbox.paypal.com/v2/checkout/orders")
        {
            Content = requestContent
        };

        requestMessage.Headers.Add("Authorization", $"Bearer {accessToken}");

        var response = await _httpClient.SendAsync(requestMessage);
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

        return createOrderResponse;
    }

    public async Task<string> GetOrderAsync(string orderId)
    {
        var accessToken = await GetAccessTokenAsync();

        var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://api-m.sandbox.paypal.com/v2/checkout/orders/{orderId}");
        requestMessage.Headers.Add("Authorization", $"Bearer {accessToken}");

        var response = await _httpClient.SendAsync(requestMessage);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        return responseContent;
    }

    public async Task<string> DeleteOrderAsync(string orderId)
    {
        var accessToken = await GetAccessTokenAsync();

        var requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"https://api-m.sandbox.paypal.com/v1/checkout/orders/{orderId}");
        requestMessage.Headers.Add("Authorization", $"Bearer {accessToken}");

        var response = await _httpClient.SendAsync(requestMessage);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        return responseContent;
    }
}
