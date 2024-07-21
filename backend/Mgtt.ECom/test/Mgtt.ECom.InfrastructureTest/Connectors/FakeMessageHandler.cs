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
}
