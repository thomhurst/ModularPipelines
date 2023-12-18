using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "cross-region-lb", "address-pool", "create")]
public record AzNetworkCrossRegionLbAddressPoolCreateOptions(
[property: CommandSwitch("--address-pool-name")] string AddressPoolName,
[property: CommandSwitch("--lb-name")] string LbName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--backend-address")]
    public string? BackendAddress { get; set; }

    [CommandSwitch("--drain-period")]
    public string? DrainPeriod { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--sync-mode")]
    public string? SyncMode { get; set; }
}