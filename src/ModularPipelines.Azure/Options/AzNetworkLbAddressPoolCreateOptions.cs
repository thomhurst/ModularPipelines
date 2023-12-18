using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "lb", "address-pool", "create")]
public record AzNetworkLbAddressPoolCreateOptions(
[property: CommandSwitch("--address-pool-name")] string AddressPoolName,
[property: CommandSwitch("--lb-name")] string LbName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--admin-state")]
    public string? AdminState { get; set; }

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

    [CommandSwitch("--tunnel-interfaces")]
    public string? TunnelInterfaces { get; set; }

    [CommandSwitch("--vnet")]
    public string? Vnet { get; set; }
}