using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "cross-region-lb", "address-pool", "create")]
public record AzNetworkCrossRegionLbAddressPoolCreateOptions(
[property: CliOption("--address-pool-name")] string AddressPoolName,
[property: CliOption("--lb-name")] string LbName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
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
}