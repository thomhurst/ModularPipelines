using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("tasks", "queues", "update")]
public record GcloudTasksQueuesUpdateOptions(
[property: PositionalArgument] string Queue
) : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--clear-log-sampling-ratio")]
    public bool? ClearLogSamplingRatio { get; set; }

    [CommandSwitch("--log-sampling-ratio")]
    public string? LogSamplingRatio { get; set; }

    [BooleanCommandSwitch("--clear-max-attempts")]
    public bool? ClearMaxAttempts { get; set; }

    [CommandSwitch("--max-attempts")]
    public string? MaxAttempts { get; set; }

    [BooleanCommandSwitch("--clear-max-backoff")]
    public bool? ClearMaxBackoff { get; set; }

    [CommandSwitch("--max-backoff")]
    public string? MaxBackoff { get; set; }

    [BooleanCommandSwitch("--clear-max-concurrent-dispatches")]
    public bool? ClearMaxConcurrentDispatches { get; set; }

    [CommandSwitch("--max-concurrent-dispatches")]
    public string? MaxConcurrentDispatches { get; set; }

    [BooleanCommandSwitch("--clear-max-dispatches-per-second")]
    public bool? ClearMaxDispatchesPerSecond { get; set; }

    [CommandSwitch("--max-dispatches-per-second")]
    public string? MaxDispatchesPerSecond { get; set; }

    [BooleanCommandSwitch("--clear-max-doublings")]
    public bool? ClearMaxDoublings { get; set; }

    [CommandSwitch("--max-doublings")]
    public string? MaxDoublings { get; set; }

    [BooleanCommandSwitch("--clear-max-retry-duration")]
    public bool? ClearMaxRetryDuration { get; set; }

    [CommandSwitch("--max-retry-duration")]
    public string? MaxRetryDuration { get; set; }

    [BooleanCommandSwitch("--clear-min-backoff")]
    public bool? ClearMinBackoff { get; set; }

    [CommandSwitch("--min-backoff")]
    public string? MinBackoff { get; set; }

    [BooleanCommandSwitch("--clear-routing-override")]
    public bool? ClearRoutingOverride { get; set; }

    [CommandSwitch("--routing-override")]
    public string[]? RoutingOverride { get; set; }
}