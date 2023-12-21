using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "firewall", "policy", "update")]
public record AzNetworkFirewallPolicyUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--auto-learn-private-ranges")]
    public string? AutoLearnPrivateRanges { get; set; }

    [CommandSwitch("--cert-name")]
    public string? CertName { get; set; }

    [CommandSwitch("--dns-servers")]
    public string? DnsServers { get; set; }

    [BooleanCommandSwitch("--enable-dns-proxy")]
    public bool? EnableDnsProxy { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--fqdns")]
    public string? Fqdns { get; set; }

    [CommandSwitch("--identity")]
    public string? Identity { get; set; }

    [CommandSwitch("--idps-mode")]
    public string? IdpsMode { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--ip-addresses")]
    public string? IpAddresses { get; set; }

    [CommandSwitch("--key-vault-secret-id")]
    public string? KeyVaultSecretId { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--private-ranges")]
    public string? PrivateRanges { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [BooleanCommandSwitch("--sql")]
    public bool? Sql { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--threat-intel-mode")]
    public string? ThreatIntelMode { get; set; }
}