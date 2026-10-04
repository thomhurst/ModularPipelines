using System.Net;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Slack.UnitTests;

public class WebhookRequestTests : TestBase
{
    private static readonly Uri WebhookUri = new("https://example.test/webhook");

    [Test]
    public async Task PostsExpectedPayloadToWebhook()
    {
        string? payload = null;
        HttpMethod? method = null;
        Uri? uri = null;
        using var client = new HttpClient(new RequestHandler(async (request, token) =>
        {
            method = request.Method;
            uri = request.RequestUri;
            payload = await request.Content!.ReadAsStringAsync(token);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }));
        var service = await GetService<ISlack>(services =>
        {
            services.RegisterSlackContext();
            services.AddSingleton<IHttpClientFactory>(new ClientFactory(client));
        });

        await service.T.PostMessageAsync(new SlackWebHookOptions(new global::Slack.Webhooks.SlackMessage { Text = "Build passed" }, WebhookUri));

        await Assert.That(method).IsEqualTo(HttpMethod.Post);
        await Assert.That(uri).IsEqualTo(WebhookUri);
        using var body = JsonDocument.Parse(payload!);
        await Assert.That(body.RootElement.GetProperty("text").GetString()).IsEqualTo("Build passed");
    }

    [Test]
    public async Task PreCanceledTokenDoesNotSendRequest()
    {
        var called = false;
        using var client = new HttpClient(new RequestHandler((_, _) =>
        {
            called = true;
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
        }));
        var service = await GetService<ISlack>(services =>
        {
            services.RegisterSlackContext();
            services.AddSingleton<IHttpClientFactory>(new ClientFactory(client));
        });
        using var cancellation = new CancellationTokenSource();
        await cancellation.CancelAsync();

        await Assert.ThrowsAsync<OperationCanceledException>(() => service.T.PostMessageAsync(new SlackWebHookOptions(new global::Slack.Webhooks.SlackMessage { Text = "Build passed" }, WebhookUri), cancellation.Token));

        await Assert.That(called).IsFalse();
    }

    [Test]
    public async Task CancellationReachesPendingHttpRequest()
    {
        var entered = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        CancellationToken requestToken = default;
        using var client = new HttpClient(new RequestHandler(async (_, token) =>
        {
            requestToken = token;
            entered.SetResult();
            await Task.Delay(Timeout.InfiniteTimeSpan, token);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }));
        var service = await GetService<ISlack>(services =>
        {
            services.RegisterSlackContext();
            services.AddSingleton<IHttpClientFactory>(new ClientFactory(client));
        });
        using var cancellation = new CancellationTokenSource();
        var posting = service.T.PostMessageAsync(new SlackWebHookOptions(new global::Slack.Webhooks.SlackMessage { Text = "Build passed" }, WebhookUri), cancellation.Token);
        try
        {
            await entered.Task.WaitAsync(TimeSpan.FromSeconds(10));
            await cancellation.CancelAsync();
            await Assert.ThrowsAsync<OperationCanceledException>(() => posting.WaitAsync(TimeSpan.FromSeconds(10)));
            await Assert.That(requestToken.IsCancellationRequested).IsTrue();
        }
        finally
        {
            await cancellation.CancelAsync();
        }
    }

    private sealed class ClientFactory(HttpClient client) : IHttpClientFactory
    {
        public HttpClient CreateClient(string name) => client;
    }

    private sealed class RequestHandler(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> send) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => send(request, cancellationToken);
    }
}
