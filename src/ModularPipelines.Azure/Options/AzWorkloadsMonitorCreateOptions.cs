using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("workloads", "monitor", "create")]
public record AzWorkloadsMonitorCreateOptions(
[property: CliOption("--monitor-name")] string MonitorName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--app-location")]
    public string? AppLocation { get; set; }

    [CliOption("--identity")]
    public string? Identity { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--log-analytics-workspace-arm-id")]
    public string? LogAnalyticsWorkspaceArmId { get; set; }

    [CliOption("--managed-rg-name")]
    public string? ManagedRgName { get; set; }

    [CliOption("--monitor-subnet")]
    public string? MonitorSubnet { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--routing-preference")]
    public string? RoutingPreference { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--zone-redundancy-preference")]
    public string? ZoneRedundancyPreference { get; set; }
}