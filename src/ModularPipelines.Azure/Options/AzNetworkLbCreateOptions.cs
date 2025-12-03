using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "lb", "create")]
public record AzNetworkLbCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--backend-pool-name")]
    public string? BackendPoolName { get; set; }

    [CliOption("--edge-zone")]
    public string? EdgeZone { get; set; }

    [CliOption("--frontend-ip-name")]
    public string? FrontendIpName { get; set; }

    [CliOption("--frontend-ip-zone")]
    public string? FrontendIpZone { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--private-ip-address")]
    public string? PrivateIpAddress { get; set; }

    [CliOption("--private-ip-address-version")]
    public string? PrivateIpAddressVersion { get; set; }

    [CliOption("--public-ip-address")]
    public string? PublicIpAddress { get; set; }

    [CliOption("--public-ip-address-allocation")]
    public string? PublicIpAddressAllocation { get; set; }

    [CliOption("--public-ip-dns-name")]
    public string? PublicIpDnsName { get; set; }

    [CliOption("--public-ip-zone")]
    public string? PublicIpZone { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliOption("--subnet-address-prefix")]
    public string? SubnetAddressPrefix { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliFlag("--validate")]
    public bool? Validate { get; set; }

    [CliOption("--vnet-address-prefix")]
    public string? VnetAddressPrefix { get; set; }

    [CliOption("--vnet-name")]
    public string? VnetName { get; set; }
}