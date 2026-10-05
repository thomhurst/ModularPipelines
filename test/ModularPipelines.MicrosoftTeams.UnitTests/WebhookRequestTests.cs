using System.Net;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.MicrosoftTeams.UnitTests;

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
        var service = await GetService<IMicrosoftTeams>(services =>
        {
            services.RegisterMicrosoftTeamsContext();
            services.AddSingleton<IHttpClientFactory>(new ClientFactory(client));
        });

        using var response = await service.T.PostCardAsync(new MicrosoftTeamsWebHookCardOptions(new MicrosoftTeamsAdaptiveCard { MsTeams = new MicrosoftTeamsProperties { Width = "Full" } }, WebhookUri));

        await Assert.That(method).IsEqualTo(HttpMethod.Post);
        await Assert.That(uri).IsEqualTo(WebhookUri);
        using var body = JsonDocument.Parse(payload!);
        await Assert.That(body.RootElement.GetProperty("type").GetString()).IsEqualTo("message");
        var attachment = body.RootElement.GetProperty("attachments")[0];
        await Assert.That(attachment.GetProperty("contentType").GetString()).IsEqualTo("application/vnd.microsoft.card.adaptive");
        await Assert.That(attachment.GetProperty("content").GetProperty("msTeams").GetProperty("width").GetString()).IsEqualTo("Full");
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
        var service = await GetService<IMicrosoftTeams>(services =>
        {
            services.RegisterMicrosoftTeamsContext();
            services.AddSingleton<IHttpClientFactory>(new ClientFactory(client));
        });
        using var cancellation = new CancellationTokenSource();
        await cancellation.CancelAsync();

        await Assert.ThrowsAsync<OperationCanceledException>(() => service.T.PostCardAsync(new MicrosoftTeamsWebHookCardOptions(new MicrosoftTeamsAdaptiveCard { MsTeams = new MicrosoftTeamsProperties { Width = "Full" } }, WebhookUri), cancellation.Token));

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
        var service = await GetService<IMicrosoftTeams>(services =>
        {
            services.RegisterMicrosoftTeamsContext();
            services.AddSingleton<IHttpClientFactory>(new ClientFactory(client));
        });
        using var cancellation = new CancellationTokenSource();
        var posting = service.T.PostCardAsync(new MicrosoftTeamsWebHookCardOptions(new MicrosoftTeamsAdaptiveCard { MsTeams = new MicrosoftTeamsProperties { Width = "Full" } }, WebhookUri), cancellation.Token);
        try
        {
            await entered.Task.WaitAsync(TestHostSettings.DefaultTestTimeout);
            await cancellation.CancelAsync();
            await Assert.ThrowsAsync<OperationCanceledException>(() => posting.WaitAsync(TestHostSettings.DefaultTestTimeout));
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
