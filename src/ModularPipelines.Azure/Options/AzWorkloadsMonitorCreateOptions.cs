using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workloads", "monitor", "create")]
public record AzWorkloadsMonitorCreateOptions(
[property: CommandSwitch("--monitor-name")] string MonitorName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--app-location")]
    public string? AppLocation { get; set; }

    [CommandSwitch("--identity")]
    public string? Identity { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--log-analytics-workspace-arm-id")]
    public string? LogAnalyticsWorkspaceArmId { get; set; }

    [CommandSwitch("--managed-rg-name")]
    public string? ManagedRgName { get; set; }

    [CommandSwitch("--monitor-subnet")]
    public string? MonitorSubnet { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--routing-preference")]
    public string? RoutingPreference { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--zone-redundancy-preference")]
    public string? ZoneRedundancyPreference { get; set; }
}

