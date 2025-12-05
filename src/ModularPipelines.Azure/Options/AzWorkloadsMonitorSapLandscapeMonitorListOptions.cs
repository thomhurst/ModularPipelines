using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("workloads", "monitor", "sap-landscape-monitor", "list")]
public record AzWorkloadsMonitorSapLandscapeMonitorListOptions(
[property: CliOption("--monitor-name")] string MonitorName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;