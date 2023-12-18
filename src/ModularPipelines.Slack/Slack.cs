using ModularPipelines.Http;
using ModularPipelines.Slack.Options;
using Slack.Webhooks;

namespace ModularPipelines.Slack;

internal class Slack : ISlack
{
    private readonly IHttp _http;

    public Slack(IHttp http)
    {
        _http = http;
    }

    public async Task PostWebHookMessage(SlackWebHookOptions options)
    {
        var slackClient = new SlackClient(options.WebHookUri.AbsoluteUri, httpClient: _http.GetLoggingHttpClient());

        await slackClient.PostAsync(options.SlackMessage);
    }
}