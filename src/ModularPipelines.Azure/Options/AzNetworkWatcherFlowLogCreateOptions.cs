using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "watcher", "flow-log", "create")]
public record AzNetworkWatcherFlowLogCreateOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliFlag("--enabled")]
    public bool? Enabled { get; set; }

    [CliOption("--format")]
    public string? Format { get; set; }

    [CliOption("--interval")]
    public int? Interval { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--log-version")]
    public string? LogVersion { get; set; }

    [CliOption("--nic")]
    public string? Nic { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--nsg")]
    public string? Nsg { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--retention")]
    public string? Retention { get; set; }

    [CliOption("--storage-account")]
    public int? StorageAccount { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliFlag("--traffic-analytics")]
    public bool? TrafficAnalytics { get; set; }

    [CliOption("--vnet")]
    public string? Vnet { get; set; }

    [CliOption("--workspace")]
    public string? Workspace { get; set; }
}