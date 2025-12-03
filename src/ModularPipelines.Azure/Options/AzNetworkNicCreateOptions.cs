using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "nic", "create")]
public record AzNetworkNicCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--subnet")] string Subnet
) : AzOptions
{
    [CliFlag("--accelerated-networking")]
    public bool? AcceleratedNetworking { get; set; }

    [CliOption("--ag-address-pools")]
    public string? AgAddressPools { get; set; }

    [CliOption("--application-security-groups")]
    public string? ApplicationSecurityGroups { get; set; }

    [CliOption("--auxiliary-mode")]
    public string? AuxiliaryMode { get; set; }

    [CliOption("--auxiliary-sku")]
    public string? AuxiliarySku { get; set; }

    [CliOption("--dns-servers")]
    public string? DnsServers { get; set; }

    [CliOption("--edge-zone")]
    public string? EdgeZone { get; set; }

    [CliOption("--gateway-name")]
    public string? GatewayName { get; set; }

    [CliOption("--internal-dns-name")]
    public string? InternalDnsName { get; set; }

    [CliFlag("--ip-forwarding")]
    public bool? IpForwarding { get; set; }

    [CliOption("--lb-address-pools")]
    public string? LbAddressPools { get; set; }

    [CliOption("--lb-inbound-nat-rules")]
    public string? LbInboundNatRules { get; set; }

    [CliOption("--lb-name")]
    public string? LbName { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--network-security-group")]
    public string? NetworkSecurityGroup { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--private-ip-address")]
    public string? PrivateIpAddress { get; set; }

    [CliOption("--private-ip-address-version")]
    public string? PrivateIpAddressVersion { get; set; }

    [CliOption("--public-ip-address")]
    public string? PublicIpAddress { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--vnet-name")]
    public string? VnetName { get; set; }
}