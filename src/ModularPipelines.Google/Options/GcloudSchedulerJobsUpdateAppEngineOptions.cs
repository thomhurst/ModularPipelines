using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("scheduler", "jobs", "update", "app-engine")]
public record GcloudSchedulerJobsUpdateAppEngineOptions(
[property: PositionalArgument] string Job,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--attempt-deadline")]
    public string? AttemptDeadline { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--http-method")]
    public string? HttpMethod { get; set; }

    [CommandSwitch("--schedule")]
    public string? Schedule { get; set; }

    [CommandSwitch("--version")]
    public new string? Version { get; set; }

    [BooleanCommandSwitch("--clear-headers")]
    public bool? ClearHeaders { get; set; }

    [CommandSwitch("--remove-headers")]
    public string[]? RemoveHeaders { get; set; }

    [CommandSwitch("--update-headers")]
    public IEnumerable<KeyValue>? UpdateHeaders { get; set; }

    [BooleanCommandSwitch("--clear-max-backoff")]
    public bool? ClearMaxBackoff { get; set; }

    [CommandSwitch("--max-backoff")]
    public string? MaxBackoff { get; set; }

    [BooleanCommandSwitch("--clear-max-doublings")]
    public bool? ClearMaxDoublings { get; set; }

    [CommandSwitch("--max-doublings")]
    public string? MaxDoublings { get; set; }

    [BooleanCommandSwitch("--clear-max-retry-attempts")]
    public bool? ClearMaxRetryAttempts { get; set; }

    [CommandSwitch("--max-retry-attempts")]
    public string? MaxRetryAttempts { get; set; }

    [BooleanCommandSwitch("--clear-max-retry-duration")]
    public bool? ClearMaxRetryDuration { get; set; }

    [CommandSwitch("--max-retry-duration")]
    public string? MaxRetryDuration { get; set; }

    [BooleanCommandSwitch("--clear-message-body")]
    public bool? ClearMessageBody { get; set; }

    [CommandSwitch("--message-body")]
    public string? MessageBody { get; set; }

    [CommandSwitch("--message-body-from-file")]
    public string? MessageBodyFromFile { get; set; }

    [BooleanCommandSwitch("--clear-min-backoff")]
    public bool? ClearMinBackoff { get; set; }

    [CommandSwitch("--min-backoff")]
    public string? MinBackoff { get; set; }

    [BooleanCommandSwitch("--clear-relative-url")]
    public bool? ClearRelativeUrl { get; set; }

    [CommandSwitch("--relative-url")]
    public string? RelativeUrl { get; set; }

    [BooleanCommandSwitch("--clear-service")]
    public bool? ClearService { get; set; }

    [CommandSwitch("--service")]
    public string? Service { get; set; }

    [BooleanCommandSwitch("--clear-time-zone")]
    public bool? ClearTimeZone { get; set; }

    [CommandSwitch("--time-zone")]
    public string? TimeZone { get; set; }
}