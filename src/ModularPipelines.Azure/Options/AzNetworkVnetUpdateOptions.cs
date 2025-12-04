using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "vnet", "update")]
public record AzNetworkVnetUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

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

    [CliFlag("--enable-encryption")]
    public bool? EnableEncryption { get; set; }

    [CliOption("--encryption-enforcement-policy")]
    public string? EncryptionEnforcementPolicy { get; set; }

    [CliOption("--flowtimeout")]
    public string? Flowtimeout { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliFlag("--vm-protection")]
    public bool? VmProtection { get; set; }
}