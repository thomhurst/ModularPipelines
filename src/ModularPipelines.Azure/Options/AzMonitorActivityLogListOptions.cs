using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "activity-log", "list")]
public record AzMonitorActivityLogListOptions : AzOptions
{
    [CliOption("--caller")]
    public string? Caller { get; set; }

    [CliOption("--correlation-id")]
    public string? CorrelationId { get; set; }

    [CliOption("--end-time")]
    public string? EndTime { get; set; }

    [CliOption("--max-events")]
    public string? MaxEvents { get; set; }

    [CliOption("--namespace")]
    public string? Namespace { get; set; }

    [CliOption("--offset")]
    public string? Offset { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--resource-id")]
    public string? ResourceId { get; set; }

    [CliOption("--select")]
    public string? Select { get; set; }

    [CliOption("--start-time")]
    public string? StartTime { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }
}