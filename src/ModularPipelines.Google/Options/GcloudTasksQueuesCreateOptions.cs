using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("tasks", "queues", "create")]
public record GcloudTasksQueuesCreateOptions(
[property: CliArgument] string Queue
) : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--log-sampling-ratio")]
    public string? LogSamplingRatio { get; set; }

    [CliOption("--max-attempts")]
    public string? MaxAttempts { get; set; }

    [CliOption("--max-backoff")]
    public string? MaxBackoff { get; set; }

    [CliOption("--max-concurrent-dispatches")]
    public string? MaxConcurrentDispatches { get; set; }

    [CliOption("--max-dispatches-per-second")]
    public string? MaxDispatchesPerSecond { get; set; }

    [CliOption("--max-doublings")]
    public string? MaxDoublings { get; set; }

    [CliOption("--max-retry-duration")]
    public string? MaxRetryDuration { get; set; }

    [CliOption("--min-backoff")]
    public string? MinBackoff { get; set; }

    [CliOption("--routing-override")]
    public string[]? RoutingOverride { get; set; }
}