using Slack.Webhooks;

namespace ModularPipelines.Slack;

public record SlackWebHookOptions(SlackMessage SlackMessage, Uri WebHookUri);