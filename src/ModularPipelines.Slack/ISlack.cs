namespace ModularPipelines.Slack;

public interface ISlack
{
    Task PostMessageAsync(SlackWebHookOptions options, CancellationToken cancellationToken = default);
}