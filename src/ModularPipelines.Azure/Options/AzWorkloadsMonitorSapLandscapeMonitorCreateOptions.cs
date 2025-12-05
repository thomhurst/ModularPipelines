using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("workloads", "monitor", "sap-landscape-monitor", "create")]
public record AzWorkloadsMonitorSapLandscapeMonitorCreateOptions(
[property: CliOption("--monitor-name")] string MonitorName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--grouping")]
    public string? Grouping { get; set; }

    [CliOption("--top-metrics-thresholds")]
    public string? TopMetricsThresholds { get; set; }
}