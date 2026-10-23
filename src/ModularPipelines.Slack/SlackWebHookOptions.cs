using System.Text;
using Slack.Webhooks;

namespace ModularPipelines.Slack;

public sealed record SlackWebHookOptions(SlackMessage SlackMessage, Uri WebHookUri)
{
    private bool PrintMembers(StringBuilder builder)
    {
        builder.Append("SlackMessage = ").Append(SlackMessage);
        builder.Append(", WebHookUri = <redacted>");
        return true;
    }
}
