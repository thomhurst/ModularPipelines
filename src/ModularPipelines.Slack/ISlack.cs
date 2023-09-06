using ModularPipelines.Slack.Options;

namespace ModularPipelines.Slack;

public interface ISlack
{
    Task PostWebHookMessage(SlackWebHookOptions options);
}