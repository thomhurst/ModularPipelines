using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vm", "monitor", "metrics", "tail")]
public record AzVmMonitorMetricsTailOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--aggregation")]
    public string? Aggregation { get; set; }

    [CliOption("--dimension")]
    public string? Dimension { get; set; }

    [CliOption("--end-time")]
    public string? EndTime { get; set; }

    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--interval")]
    public int? Interval { get; set; }

    [CliOption("--metadata")]
    public string? Metadata { get; set; }

    [CliOption("--metrics")]
    public string? Metrics { get; set; }

    [CliOption("--namespace")]
    public string? Namespace { get; set; }

    [CliOption("--offset")]
    public string? Offset { get; set; }

    [CliOption("--orderby")]
    public string? Orderby { get; set; }

    [CliOption("--start-time")]
    public string? StartTime { get; set; }

    [CliOption("--top")]
    public string? Top { get; set; }
}