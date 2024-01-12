using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("tasks", "queues", "create")]
public record GcloudTasksQueuesCreateOptions(
[property: PositionalArgument] string Queue
) : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--log-sampling-ratio")]
    public string? LogSamplingRatio { get; set; }

    [CommandSwitch("--max-attempts")]
    public string? MaxAttempts { get; set; }

    [CommandSwitch("--max-backoff")]
    public string? MaxBackoff { get; set; }

    [CommandSwitch("--max-concurrent-dispatches")]
    public string? MaxConcurrentDispatches { get; set; }

    [CommandSwitch("--max-dispatches-per-second")]
    public string? MaxDispatchesPerSecond { get; set; }

    [CommandSwitch("--max-doublings")]
    public string? MaxDoublings { get; set; }

    [CommandSwitch("--max-retry-duration")]
    public string? MaxRetryDuration { get; set; }

    [CommandSwitch("--min-backoff")]
    public string? MinBackoff { get; set; }

    [CommandSwitch("--routing-override")]
    public string[]? RoutingOverride { get; set; }
}