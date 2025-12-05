using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("tasks", "queues", "update")]
public record GcloudTasksQueuesUpdateOptions(
[property: CliArgument] string Queue
) : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--clear-log-sampling-ratio")]
    public bool? ClearLogSamplingRatio { get; set; }

    [CliOption("--log-sampling-ratio")]
    public string? LogSamplingRatio { get; set; }

    [CliFlag("--clear-max-attempts")]
    public bool? ClearMaxAttempts { get; set; }

    [CliOption("--max-attempts")]
    public string? MaxAttempts { get; set; }

    [CliFlag("--clear-max-backoff")]
    public bool? ClearMaxBackoff { get; set; }

    [CliOption("--max-backoff")]
    public string? MaxBackoff { get; set; }

    [CliFlag("--clear-max-concurrent-dispatches")]
    public bool? ClearMaxConcurrentDispatches { get; set; }

    [CliOption("--max-concurrent-dispatches")]
    public string? MaxConcurrentDispatches { get; set; }

    [CliFlag("--clear-max-dispatches-per-second")]
    public bool? ClearMaxDispatchesPerSecond { get; set; }

    [CliOption("--max-dispatches-per-second")]
    public string? MaxDispatchesPerSecond { get; set; }

    [CliFlag("--clear-max-doublings")]
    public bool? ClearMaxDoublings { get; set; }

    [CliOption("--max-doublings")]
    public string? MaxDoublings { get; set; }

    [CliFlag("--clear-max-retry-duration")]
    public bool? ClearMaxRetryDuration { get; set; }

    [CliOption("--max-retry-duration")]
    public string? MaxRetryDuration { get; set; }

    [CliFlag("--clear-min-backoff")]
    public bool? ClearMinBackoff { get; set; }

    [CliOption("--min-backoff")]
    public string? MinBackoff { get; set; }

    [CliFlag("--clear-routing-override")]
    public bool? ClearRoutingOverride { get; set; }

    [CliOption("--routing-override")]
    public string[]? RoutingOverride { get; set; }
}