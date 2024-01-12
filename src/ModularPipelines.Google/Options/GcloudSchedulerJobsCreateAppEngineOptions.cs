using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("scheduler", "jobs", "create", "app-engine")]
public record GcloudSchedulerJobsCreateAppEngineOptions(
[property: PositionalArgument] string Job,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--schedule")] string Schedule
) : GcloudOptions
{
    [CommandSwitch("--attempt-deadline")]
    public string? AttemptDeadline { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--headers")]
    public IEnumerable<KeyValue>? Headers { get; set; }

    [CommandSwitch("--http-method")]
    public string? HttpMethod { get; set; }

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

    [CommandSwitch("--relative-url")]
    public string? RelativeUrl { get; set; }

    [CommandSwitch("--service")]
    public string? Service { get; set; }

    [CommandSwitch("--time-zone")]
    public string? TimeZone { get; set; }

    [CommandSwitch("--version")]
    public new string? Version { get; set; }

    [CommandSwitch("--message-body")]
    public string? MessageBody { get; set; }

    [CommandSwitch("--message-body-from-file")]
    public string? MessageBodyFromFile { get; set; }
}