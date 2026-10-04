using System.Net;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Exceptions;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Slack.UnitTests;

public class SlackResponseTests : TestBase
{
    [Test]
    [Arguments(HttpStatusCode.OK)]
    [Arguments(HttpStatusCode.BadRequest)]
    [Arguments(HttpStatusCode.InternalServerError)]
    public async Task WebhookFailuresThrowAndResponsesAreDisposed(HttpStatusCode status)
    {
        var content = new TrackingContent();
        using var client = new HttpClient(new ResponseHandler(status, content));
        var service = await GetService<ISlack>(services =>
        {
            services.RegisterSlackContext();
            services.AddSingleton<IHttpClientFactory>(new ClientFactory(client));
        });
        var options = new SlackWebHookOptions(new global::Slack.Webhooks.SlackMessage { Text = "Build passed" }, new Uri("https://example.test/webhook"));

        if (status == HttpStatusCode.OK)
        {
            await service.T.PostMessageAsync(options);
        }
        else
        {
            var exception = await Assert.ThrowsAsync<PipelineHttpResponseException>(() => service.T.PostMessageAsync(options));
            await Assert.That(exception!.StatusCode).IsEqualTo(status);
            await Assert.That(exception.ResponseContent).IsEqualTo("webhook response");
        }

        await Assert.That(content.IsDisposed).IsTrue();
    }

    private sealed class TrackingContent() : StringContent("webhook response")
    {
        public bool IsDisposed { get; private set; }

        protected override void Dispose(bool disposing)
        {
            IsDisposed = true;
            base.Dispose(disposing);
        }
    }

    private sealed class ClientFactory(HttpClient client) : IHttpClientFactory
    {
        public HttpClient CreateClient(string name) => client;
    }

    private sealed class ResponseHandler(HttpStatusCode status, HttpContent content) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => Task.FromResult(new HttpResponseMessage(status) { Content = content });
    }
}
