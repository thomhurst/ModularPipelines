using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "firewall", "policy", "intrusion-detection", "remove")]
public record AzNetworkFirewallPolicyIntrusionDetectionRemoveOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--auto-learn-private-ranges")]
    public string? AutoLearnPrivateRanges { get; set; }

    [CliOption("--cert-name")]
    public string? CertName { get; set; }

    [CliOption("--configuration")]
    public string? Configuration { get; set; }

    [CliOption("--dns-servers")]
    public string? DnsServers { get; set; }

    [CliFlag("--enable-dns-proxy")]
    public bool? EnableDnsProxy { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--fqdns")]
    public string? Fqdns { get; set; }

    [CliOption("--identity-type")]
    public string? IdentityType { get; set; }

    [CliOption("--idps-mode")]
    public string? IdpsMode { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--ip-addresses")]
    public string? IpAddresses { get; set; }

    [CliOption("--key-vault-secret-id")]
    public string? KeyVaultSecretId { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--policy-name")]
    public string? PolicyName { get; set; }

    [CliOption("--private-ranges")]
    public string? PrivateRanges { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--rule-name")]
    public string? RuleName { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--signature-id")]
    public string? SignatureId { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliFlag("--sql")]
    public bool? Sql { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--threat-intel-mode")]
    public string? ThreatIntelMode { get; set; }
}