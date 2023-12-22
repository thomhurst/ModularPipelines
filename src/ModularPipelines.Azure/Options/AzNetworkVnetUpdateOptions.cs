using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vnet", "update")]
public record AzNetworkVnetUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

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

    [BooleanCommandSwitch("--enable-encryption")]
    public bool? EnableEncryption { get; set; }

    [CommandSwitch("--encryption-enforcement-policy")]
    public string? EncryptionEnforcementPolicy { get; set; }

    [CommandSwitch("--flowtimeout")]
    public string? Flowtimeout { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [BooleanCommandSwitch("--vm-protection")]
    public bool? VmProtection { get; set; }
}