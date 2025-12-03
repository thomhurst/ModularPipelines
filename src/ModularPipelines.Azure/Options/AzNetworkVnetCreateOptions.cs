using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "vnet", "create")]
public record AzNetworkVnetCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--address-prefixes")]
    public string? AddressPrefixes { get; set; }

    [CliOption("--bgp-community")]
    public string? BgpCommunity { get; set; }

    [CliFlag("--ddos-protection")]
    public bool? DdosProtection { get; set; }

    [CliOption("--ddos-protection-plan")]
    public string? DdosProtectionPlan { get; set; }

    [CliOption("--dns-servers")]
    public string? DnsServers { get; set; }

    [CliOption("--edge-zone")]
    public string? EdgeZone { get; set; }

    [CliFlag("--enable-encryption")]
    public bool? EnableEncryption { get; set; }

    [CliOption("--encryption-enforcement-policy")]
    public string? EncryptionEnforcementPolicy { get; set; }

    [CliOption("--flowtimeout")]
    public string? Flowtimeout { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--network-security-group")]
    public string? NetworkSecurityGroup { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--subnet-name")]
    public string? SubnetName { get; set; }

    [CliOption("--subnet-prefixes")]
    public string? SubnetPrefixes { get; set; }

    [CliOption("--subnets")]
    public string? Subnets { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliFlag("--vm-protection")]
    public bool? VmProtection { get; set; }
}