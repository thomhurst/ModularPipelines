using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "lb", "address-pool", "update")]
public record AzNetworkLbAddressPoolUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--address-pool-name")]
    public string? AddressPoolName { get; set; }

    [CliOption("--admin-state")]
    public string? AdminState { get; set; }

    [CliOption("--backend-address")]
    public string? BackendAddress { get; set; }

    [CliOption("--drain-period")]
    public string? DrainPeriod { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--lb-name")]
    public string? LbName { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--sync-mode")]
    public string? SyncMode { get; set; }

    [CliOption("--tunnel-interfaces")]
    public string? TunnelInterfaces { get; set; }

    [CliOption("--vnet")]
    public string? Vnet { get; set; }
}