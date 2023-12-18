using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "watcher", "flow-log", "create")]
public record AzNetworkWatcherFlowLogCreateOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [BooleanCommandSwitch("--enabled")]
    public bool? Enabled { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }

    [CommandSwitch("--interval")]
    public int? Interval { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--log-version")]
    public string? LogVersion { get; set; }

    [CommandSwitch("--nic")]
    public string? Nic { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--nsg")]
    public string? Nsg { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--retention")]
    public string? Retention { get; set; }

    [CommandSwitch("--storage-account")]
    public int? StorageAccount { get; set; }

    [CommandSwitch("--subnet")]
    public string? Subnet { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [BooleanCommandSwitch("--traffic-analytics")]
    public bool? TrafficAnalytics { get; set; }

    [CommandSwitch("--vnet")]
    public string? Vnet { get; set; }

    [CommandSwitch("--workspace")]
    public string? Workspace { get; set; }
}