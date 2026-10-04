using System.Net;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Exceptions;
using ModularPipelines.MicrosoftTeams.Extensions;
using ModularPipelines.MicrosoftTeams.Models;
using ModularPipelines.MicrosoftTeams.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.MicrosoftTeams.UnitTests;

public class MicrosoftTeamsResponseTests : TestBase
{
    [Test]
    [Arguments(HttpStatusCode.OK, false)]
    [Arguments(HttpStatusCode.OK, true)]
    [Arguments(HttpStatusCode.BadRequest, false)]
    [Arguments(HttpStatusCode.BadRequest, true)]
    [Arguments(HttpStatusCode.InternalServerError, false)]
    [Arguments(HttpStatusCode.InternalServerError, true)]
    public async Task WebhookFailuresThrowByDefaultAndAllowResponseInspection(HttpStatusCode status, bool optOut)
    {
        using var client = new HttpClient(new ResponseHandler(status));
        var service = await GetService<IMicrosoftTeams>(services =>
        {
            services.RegisterMicrosoftTeamsContext();
            services.AddSingleton<IHttpClientFactory>(new ClientFactory(client));
        });
        var options = new MicrosoftTeamsWebHookCardOptions(new MicrosoftTeamsAdaptiveCard(), new Uri("https://example.test/webhook"));
        if (optOut)
        {
            options = options with { ThrowOnNonSuccessStatusCode = false };
        }

        if (status != HttpStatusCode.OK && !optOut)
        {
            var exception = await Assert.ThrowsAsync<PipelineHttpResponseException>(() => service.T.PostMicrosoftTeamsCard(options));
            await Assert.That(exception!.StatusCode).IsEqualTo(status);
            await Assert.That(exception.ResponseContent).IsEqualTo("webhook response");
        }
        else
        {
            using var response = await service.T.PostMicrosoftTeamsCard(options);
            await Assert.That(response.StatusCode).IsEqualTo(status);
            await Assert.That(await response.Content.ReadAsStringAsync()).IsEqualTo("webhook response");
        }
    }

    private sealed class ClientFactory(HttpClient client) : IHttpClientFactory
    {
        public HttpClient CreateClient(string name) => client;
    }

    private sealed class ResponseHandler(HttpStatusCode status) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => Task.FromResult(new HttpResponseMessage(status) { Content = new StringContent("webhook response") });
    }
}
