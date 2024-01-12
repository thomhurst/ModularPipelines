using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("scheduler", "jobs", "create", "pubsub")]
public record GcloudSchedulerJobsCreatePubsubOptions(
[property: PositionalArgument] string Job,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--schedule")] string Schedule,
[property: CommandSwitch("--topic")] string Topic,
[property: CommandSwitch("--attributes")] string Attributes,
[property: CommandSwitch("--message-body")] string MessageBody,
[property: CommandSwitch("--message-body-from-file")] string MessageBodyFromFile
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--max-backoff")]
    public string? MaxBackoff { get; set; }

    [CommandSwitch("--max-doublings")]
    public string? MaxDoublings { get; set; }

    [CommandSwitch("--max-retry-attempts")]
    public string? MaxRetryAttempts { get; set; }

    [CommandSwitch("--max-retry-duration")]
    public string? MaxRetryDuration { get; set; }

    [CommandSwitch("--min-backoff")]
    public string? MinBackoff { get; set; }

    [CommandSwitch("--time-zone")]
    public string? TimeZone { get; set; }
}