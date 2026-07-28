using ModularPipelines.Context.Domains.Network;
using ModularPipelines.Http;
using ModularPipelines.Slack.Options;
using Slack.Webhooks;

namespace ModularPipelines.Slack;

internal class Slack : ISlack
{
    private readonly IHttpContext _http;

    public Slack(IHttpContext http)
    {
        _http = http;
    }

    public async Task PostWebHookMessage(SlackWebHookOptions options, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var slackClient = new SlackClient(options.WebHookUri.AbsoluteUri, httpClient: _http.GetLoggingHttpClient());

        await slackClient.PostAsync(options.SlackMessage);
    }
}
