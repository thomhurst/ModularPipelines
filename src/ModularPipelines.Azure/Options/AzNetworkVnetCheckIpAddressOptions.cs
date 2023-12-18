using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vnet", "check-ip-address")]
public record AzNetworkVnetCheckIpAddressOptions(
[property: CommandSwitch("--ip-address")] string IpAddress,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--address-prefixes")]
    public string? AddressPrefixes { get; set; }

    [CommandSwitch("--bgp-community")]
    public string? BgpCommunity { get; set; }

    [BooleanCommandSwitch("--ddos-protection")]
    public bool? DdosProtection { get; set; }

    [CommandSwitch("--ddos-protection-plan")]
    public string? DdosProtectionPlan { get; set; }

    [CommandSwitch("--dns-servers")]
    public string? DnsServers { get; set; }

    [CommandSwitch("--edge-zone")]
    public string? EdgeZone { get; set; }

    [BooleanCommandSwitch("--enable-encryption")]
    public bool? EnableEncryption { get; set; }

    [CommandSwitch("--encryption-enforcement-policy")]
    public string? EncryptionEnforcementPolicy { get; set; }

    [CommandSwitch("--flowtimeout")]
    public string? Flowtimeout { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--network-security-group")]
    public string? NetworkSecurityGroup { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--subnet-name")]
    public string? SubnetName { get; set; }

    [CommandSwitch("--subnet-prefixes")]
    public string? SubnetPrefixes { get; set; }

    [CommandSwitch("--subnets")]
    public string? Subnets { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [BooleanCommandSwitch("--vm-protection")]
    public bool? VmProtection { get; set; }
}