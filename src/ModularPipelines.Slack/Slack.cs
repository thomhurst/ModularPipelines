using System.Text;
using ModularPipelines.Context;
using Slack.Webhooks;

namespace ModularPipelines.Slack;

internal class Slack : ISlack
{
    private readonly IHttpContext _http;

    public Slack(IHttpContext http)
    {
        _http = http;
    }

    public async Task PostMessageAsync(SlackWebHookOptions options, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using var request = new HttpRequestMessage(HttpMethod.Post, options.WebHookUri)
        {
            Content = new StringContent(
                SlackClient.SerializeObject(options.SlackMessage),
                Encoding.UTF8,
                "application/json"),
        };

        using var response = await _http.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}
