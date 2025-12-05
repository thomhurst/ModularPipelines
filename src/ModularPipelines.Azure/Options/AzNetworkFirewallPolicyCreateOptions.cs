using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "firewall", "policy", "create")]
public record AzNetworkFirewallPolicyCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--auto-learn-private-ranges")]
    public string? AutoLearnPrivateRanges { get; set; }

    [CliOption("--base-policy")]
    public string? BasePolicy { get; set; }

    [CliOption("--cert-name")]
    public string? CertName { get; set; }

    [CliOption("--dns-servers")]
    public string? DnsServers { get; set; }

    [CliFlag("--enable-dns-proxy")]
    public bool? EnableDnsProxy { get; set; }

    [CliOption("--fqdns")]
    public string? Fqdns { get; set; }

    [CliOption("--identity")]
    public string? Identity { get; set; }

    [CliOption("--idps-mode")]
    public string? IdpsMode { get; set; }

    [CliOption("--ip-addresses")]
    public string? IpAddresses { get; set; }

    [CliOption("--key-vault-secret-id")]
    public string? KeyVaultSecretId { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--private-ranges")]
    public string? PrivateRanges { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliFlag("--sql")]
    public bool? Sql { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--threat-intel-mode")]
    public string? ThreatIntelMode { get; set; }
}