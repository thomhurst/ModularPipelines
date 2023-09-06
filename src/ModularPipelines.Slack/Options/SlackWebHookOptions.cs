using Slack.Webhooks;

namespace ModularPipelines.Slack.Options;

public record SlackWebHookOptions(SlackMessage SlackMessage, Uri WebHookUri);