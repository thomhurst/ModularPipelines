using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "cross-region-lb", "create")]
public record AzNetworkCrossRegionLbCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--address-allocation")]
    public string? AddressAllocation { get; set; }

    [CliOption("--backend-pool-name")]
    public string? BackendPoolName { get; set; }

    [CliOption("--frontend-ip-name")]
    public string? FrontendIpName { get; set; }

    [CliOption("--frontend-ip-zone")]
    public string? FrontendIpZone { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--public-ip-address")]
    public string? PublicIpAddress { get; set; }

    [CliOption("--public-ip-dns-name")]
    public string? PublicIpDnsName { get; set; }

    [CliOption("--public-ip-zone")]
    public string? PublicIpZone { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliFlag("--validate")]
    public bool? Validate { get; set; }
}