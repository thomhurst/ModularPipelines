using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("scheduler", "jobs", "update", "pubsub")]
public record GcloudSchedulerJobsUpdatePubsubOptions(
[property: PositionalArgument] string Job,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--schedule")]
    public string? Schedule { get; set; }

    [CommandSwitch("--topic")]
    public string? Topic { get; set; }

    [BooleanCommandSwitch("--clear-attributes")]
    public bool? ClearAttributes { get; set; }

    [CommandSwitch("--remove-attributes")]
    public string[]? RemoveAttributes { get; set; }

    [CommandSwitch("--update-attributes")]
    public IEnumerable<KeyValue>? UpdateAttributes { get; set; }

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

    [BooleanCommandSwitch("--clear-min-backoff")]
    public bool? ClearMinBackoff { get; set; }

    [CommandSwitch("--min-backoff")]
    public string? MinBackoff { get; set; }

    [BooleanCommandSwitch("--clear-time-zone")]
    public bool? ClearTimeZone { get; set; }

    [CommandSwitch("--time-zone")]
    public string? TimeZone { get; set; }

    [CommandSwitch("--message-body")]
    public string? MessageBody { get; set; }

    [CommandSwitch("--message-body-from-file")]
    public string? MessageBodyFromFile { get; set; }
}