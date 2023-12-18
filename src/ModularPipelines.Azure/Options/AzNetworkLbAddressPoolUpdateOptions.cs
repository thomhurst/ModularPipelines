using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "lb", "address-pool", "update")]
public record AzNetworkLbAddressPoolUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--address-pool-name")]
    public string? AddressPoolName { get; set; }

    [CommandSwitch("--admin-state")]
    public string? AdminState { get; set; }

    [CommandSwitch("--backend-address")]
    public string? BackendAddress { get; set; }

    [CommandSwitch("--drain-period")]
    public string? DrainPeriod { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--lb-name")]
    public string? LbName { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--sync-mode")]
    public string? SyncMode { get; set; }

    [CommandSwitch("--tunnel-interfaces")]
    public string? TunnelInterfaces { get; set; }

    [CommandSwitch("--vnet")]
    public string? Vnet { get; set; }
}

