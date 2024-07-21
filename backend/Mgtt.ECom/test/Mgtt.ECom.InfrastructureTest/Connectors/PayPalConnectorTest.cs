using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Mgtt.ECom.Infrastructure.Connectors;
using Mgtt.ECom.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Xunit;

namespace Mgtt.ECom.InfrastructureTest.Connectors
{
    public class PayPalConnectorTest
{
    private readonly string orderId;
    
    private readonly IOptions<PayPalSettings> settings;

    private readonly ILogger<PayPalConnector> mockLogger;

    public PayPalConnectorTest()
    {
        var fakeHttpMessageHandler = new FakeHttpMessageHandler(request =>
        {
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        });

        var httpClient = new HttpClient(fakeHttpMessageHandler);
        var clientId = Guid.NewGuid().ToString();
        var clientSecret = Guid.NewGuid().ToString();
        var baseUrl = "https://api-m.sandbox.paypal.com";

        this.settings = Options.Create(new PayPalSettings
        {
            ClientId = clientId,
            ClientSecret = clientSecret,
            BaseUrl = baseUrl,
        });
        this.mockLogger = Mock.Of<ILogger<PayPalConnector>>();
    }

    [Fact]
    public async Task GetAccessTokenAsync_ReturnsAccessToken()
    {
        var expectedAccessToken = "test-access-token";
        var responseContent = JsonConvert.SerializeObject(new { access_token = expectedAccessToken });
        var fakeHttpMessageHandler = new FakeHttpMessageHandler(request =>
        {
            if (request.Method == HttpMethod.Post && request.RequestUri.ToString().Contains("/v1/oauth2/token"))
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(responseContent),
                };
            }
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        });

        var httpClient = new HttpClient(fakeHttpMessageHandler);
        var connector = new PayPalConnector(httpClient, this.settings, this.mockLogger);

        var accessToken = await connector.GetAccessTokenAsync();

        Assert.Equal(expectedAccessToken, accessToken);
    }

    [Fact]
    public async Task CreateOrderAsync_ReturnsCreateOrderResponse()
    {
        // Arrange
        var orderDetails = new OrderDetails
        {
            TotalAmount = 0.50m,
            CurrencyCode = "USD",
            ReferenceId = "test-reference-id",
            AddressLine1 = "123 Main St",
            AddressLine2 = "Apt 4B",
            AdminArea2 = "San Jose",
            AdminArea1 = "CA",
            PostalCode = "95131",
            CountryCode = "US",
        };

        var expectedOrderId = "test-order-id";
        var expectedAccessToken = "test-access-token";
    
        var responseContent = JsonConvert.SerializeObject(new
        {
            id = expectedOrderId,
            links = new[]
            {
                new { href = "https://www.sandbox.paypal.com/checkoutnow?token=test-order-id", rel = "checkoutnow" },
            },
            access_token = expectedAccessToken,
        });

        var fakeHttpMessageHandler = new FakeHttpMessageHandler(request =>
        {
            if (request.Method == HttpMethod.Post)
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(responseContent),
                };
            }
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        });

        var httpClient = new HttpClient(fakeHttpMessageHandler);
        var connector = new PayPalConnector(httpClient, this.settings, this.mockLogger);

        var createOrderResponse = await connector.CreateOrderAsync(orderDetails);

        Assert.NotNull(createOrderResponse);
        Assert.Equal(expectedOrderId, createOrderResponse.OrderId);
    }


}

}