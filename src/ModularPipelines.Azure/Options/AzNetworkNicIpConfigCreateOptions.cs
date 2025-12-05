using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "nic", "ip-config", "create")]
public record AzNetworkNicIpConfigCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--nic-name")] string NicName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--ag-address-pools")]
    public string? AgAddressPools { get; set; }

    [CliOption("--application-security-groups")]
    public string? ApplicationSecurityGroups { get; set; }

    [CliOption("--gateway-name")]
    public string? GatewayName { get; set; }

    [CliOption("--lb-address-pools")]
    public string? LbAddressPools { get; set; }

    [CliOption("--lb-inbound-nat-rules")]
    public string? LbInboundNatRules { get; set; }

    [CliOption("--lb-name")]
    public string? LbName { get; set; }

    [CliFlag("--make-primary")]
    public bool? MakePrimary { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--private-ip-address")]
    public string? PrivateIpAddress { get; set; }

    [CliOption("--private-ip-address-version")]
    public string? PrivateIpAddressVersion { get; set; }

    [CliOption("--public-ip-address")]
    public string? PublicIpAddress { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliOption("--vnet-name")]
    public string? VnetName { get; set; }
}