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
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, HttpResponseMessage> responseHandler;

        public FakeHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> responseHandler)
        {
            this.responseHandler = responseHandler;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = this.responseHandler(request);
            return Task.FromResult(response);
        }
    }

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

        var accessToken = await connector.GetAccessTokenAsync();

        Assert.Equal(expectedAccessToken, accessToken);
    }

    [Fact]
    public async Task CreateOrderAsync_ReturnsCreateOrderResponse()
    {
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
    
        var responseContent = JsonConvert.SerializeObject(new
        {
            id = expectedOrderId,
            links = new[]
            {
                new { href = "https://www.sandbox.paypal.com/checkoutnow?token=test-order-id", rel = "checkoutnow" },
            },
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

        var fakeAccessToken = "test-access-token";
        var createOrderResponse = await connector.CreateOrderAsync(orderDetails, fakeAccessToken);

        Assert.NotNull(createOrderResponse);
        Assert.Equal(expectedOrderId, createOrderResponse.OrderId);
    }

    [Fact]
    public async Task GetOrderAsync_ReturnsOrderDetails()
    {
        var expectedOrderId = "test-order-id";

        var expectedOrderDetails = new
        {
            id = expectedOrderId,
            status = "CREATED",
        };

        var responseContent = JsonConvert.SerializeObject(expectedOrderDetails);
        var fakeHttpMessageHandler = new FakeHttpMessageHandler(request =>
        {
            if (request.Method == HttpMethod.Get)
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

        var fakeAccessToken = "test-access-token"; // In practice, connector.GetAccessTokenAsync() would be invoked as a prerequisite
        var orderResponse = await connector.GetOrderAsync(expectedOrderId, fakeAccessToken);

        Assert.NotNull(orderResponse);
        Assert.Contains(expectedOrderId, orderResponse);
    }

    [Fact]
    public async Task GetOrderAsync_ReturnsNullWhenAccessTokenIsMissing()
    {
        var expectedOrderId = "test-order-id";
        var fakeHttpMessageHandler = new FakeHttpMessageHandler(request =>
        {
            if (request.Method == HttpMethod.Get)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        });

        var httpClient = new HttpClient(fakeHttpMessageHandler);
        var connector = new PayPalConnector(httpClient, this.settings, this.mockLogger);

        var fakeAccessToken = "test-access-token"; // In practice, connector.GetAccessTokenAsync() would be invoked as a prerequisite
        var orderResponse = await connector.GetOrderAsync(expectedOrderId, fakeAccessToken);

        Assert.Null(orderResponse);
    }

    [Fact]
    public async Task DeleteOrderAsync_ReturnsOrderDetails()
    {
        var expectedOrderId = "test-order-id";

        var fakeHttpMessageHandler = new FakeHttpMessageHandler(request =>
        {
            if (request.Method == HttpMethod.Delete)
            {
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        });

        var httpClient = new HttpClient(fakeHttpMessageHandler);
        var connector = new PayPalConnector(httpClient, this.settings, this.mockLogger);

        var fakeAccessToken = "test-access-token"; // In practice, connector.GetAccessTokenAsync() would be invoked as a prerequisite
        var deleteOrderResponse = await connector.DeleteOrderAsync(expectedOrderId, fakeAccessToken);

        Assert.True(deleteOrderResponse);
    }
    }
}