using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "nic", "ip-config", "update")]
public record AzNetworkNicIpConfigUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--nic-name")] string NicName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--ag-address-pools")]
    public string? AgAddressPools { get; set; }

    [CliOption("--application-security-groups")]
    public string? ApplicationSecurityGroups { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--gateway-lb")]
    public string? GatewayLb { get; set; }

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

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliOption("--vnet-name")]
    public string? VnetName { get; set; }
}