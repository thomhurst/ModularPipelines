using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "nic", "create")]
public record AzNetworkNicCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--subnet")] string Subnet
) : AzOptions
{
    [BooleanCommandSwitch("--accelerated-networking")]
    public bool? AcceleratedNetworking { get; set; }

    [CommandSwitch("--ag-address-pools")]
    public string? AgAddressPools { get; set; }

    [CommandSwitch("--application-security-groups")]
    public string? ApplicationSecurityGroups { get; set; }

    [CommandSwitch("--auxiliary-mode")]
    public string? AuxiliaryMode { get; set; }

    [CommandSwitch("--auxiliary-sku")]
    public string? AuxiliarySku { get; set; }

    [CommandSwitch("--dns-servers")]
    public string? DnsServers { get; set; }

    [CommandSwitch("--edge-zone")]
    public string? EdgeZone { get; set; }

    [CommandSwitch("--gateway-name")]
    public string? GatewayName { get; set; }

    [CommandSwitch("--internal-dns-name")]
    public string? InternalDnsName { get; set; }

    [BooleanCommandSwitch("--ip-forwarding")]
    public bool? IpForwarding { get; set; }

    [CommandSwitch("--lb-address-pools")]
    public string? LbAddressPools { get; set; }

    [CommandSwitch("--lb-inbound-nat-rules")]
    public string? LbInboundNatRules { get; set; }

    [CommandSwitch("--lb-name")]
    public string? LbName { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--network-security-group")]
    public string? NetworkSecurityGroup { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--private-ip-address")]
    public string? PrivateIpAddress { get; set; }

    [CommandSwitch("--private-ip-address-version")]
    public string? PrivateIpAddressVersion { get; set; }

    [CommandSwitch("--public-ip-address")]
    public string? PublicIpAddress { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--vnet-name")]
    public string? VnetName { get; set; }
}