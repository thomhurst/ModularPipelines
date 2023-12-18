using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "firewall", "policy", "create")]
public record AzNetworkFirewallPolicyCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--auto-learn-private-ranges")]
    public string? AutoLearnPrivateRanges { get; set; }

    [CommandSwitch("--base-policy")]
    public string? BasePolicy { get; set; }

    [CommandSwitch("--cert-name")]
    public string? CertName { get; set; }

    [CommandSwitch("--dns-servers")]
    public string? DnsServers { get; set; }

    [BooleanCommandSwitch("--enable-dns-proxy")]
    public bool? EnableDnsProxy { get; set; }

    [CommandSwitch("--fqdns")]
    public string? Fqdns { get; set; }

    [CommandSwitch("--identity")]
    public string? Identity { get; set; }

    [CommandSwitch("--idps-mode")]
    public string? IdpsMode { get; set; }

    [CommandSwitch("--ip-addresses")]
    public string? IpAddresses { get; set; }

    [CommandSwitch("--key-vault-secret-id")]
    public string? KeyVaultSecretId { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--private-ranges")]
    public string? PrivateRanges { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [BooleanCommandSwitch("--sql")]
    public bool? Sql { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--threat-intel-mode")]
    public string? ThreatIntelMode { get; set; }
}