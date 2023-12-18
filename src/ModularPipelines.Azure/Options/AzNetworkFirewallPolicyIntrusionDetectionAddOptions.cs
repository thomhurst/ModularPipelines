using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "firewall", "policy", "intrusion-detection", "add")]
public record AzNetworkFirewallPolicyIntrusionDetectionAddOptions(
[property: CommandSwitch("--policy-name")] string PolicyName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--auto-learn-private-ranges")]
    public string? AutoLearnPrivateRanges { get; set; }

    [CommandSwitch("--cert-name")]
    public string? CertName { get; set; }

    [CommandSwitch("--configuration")]
    public string? Configuration { get; set; }

    [CommandSwitch("--dns-servers")]
    public string? DnsServers { get; set; }

    [BooleanCommandSwitch("--enable-dns-proxy")]
    public bool? EnableDnsProxy { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--fqdns")]
    public string? Fqdns { get; set; }

    [CommandSwitch("--identity-type")]
    public string? IdentityType { get; set; }

    [CommandSwitch("--idps-mode")]
    public string? IdpsMode { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--ip-addresses")]
    public string? IpAddresses { get; set; }

    [CommandSwitch("--key-vault-secret-id")]
    public string? KeyVaultSecretId { get; set; }

    [CommandSwitch("--mode")]
    public string? Mode { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--private-ranges")]
    public string? PrivateRanges { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--rule-description")]
    public string? RuleDescription { get; set; }

    [CommandSwitch("--rule-dest-addresses")]
    public string? RuleDestAddresses { get; set; }

    [CommandSwitch("--rule-dest-ip-groups")]
    public string? RuleDestIpGroups { get; set; }

    [CommandSwitch("--rule-dest-ports")]
    public string? RuleDestPorts { get; set; }

    [CommandSwitch("--rule-name")]
    public string? RuleName { get; set; }

    [CommandSwitch("--rule-protocol")]
    public string? RuleProtocol { get; set; }

    [CommandSwitch("--rule-src-addresses")]
    public string? RuleSrcAddresses { get; set; }

    [CommandSwitch("--rule-src-ip-groups")]
    public string? RuleSrcIpGroups { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--signature-id")]
    public string? SignatureId { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [BooleanCommandSwitch("--sql")]
    public bool? Sql { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--threat-intel-mode")]
    public string? ThreatIntelMode { get; set; }
}

