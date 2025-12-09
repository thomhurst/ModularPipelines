using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "lb", "address-pool", "create")]
public record AzNetworkLbAddressPoolCreateOptions(
[property: CliOption("--address-pool-name")] string AddressPoolName,
[property: CliOption("--lb-name")] string LbName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--admin-state")]
    public string? AdminState { get; set; }

    [CliOption("--backend-address")]
    public string? BackendAddress { get; set; }

    [CliOption("--drain-period")]
    public string? DrainPeriod { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--sync-mode")]
    public string? SyncMode { get; set; }

    [CliOption("--tunnel-interfaces")]
    public string? TunnelInterfaces { get; set; }

    [CliOption("--vnet")]
    public string? Vnet { get; set; }
}