using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workloads", "monitor", "sap-landscape-monitor", "create")]
public record AzWorkloadsMonitorSapLandscapeMonitorCreateOptions(
[property: CommandSwitch("--monitor-name")] string MonitorName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--grouping")]
    public string? Grouping { get; set; }

    [CommandSwitch("--top-metrics-thresholds")]
    public string? TopMetricsThresholds { get; set; }
}