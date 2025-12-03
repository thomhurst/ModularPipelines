using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "network-firewall-policies", "rules", "create")]
public record GcloudComputeNetworkFirewallPoliciesRulesCreateOptions(
[property: CliArgument] string Priority,
[property: CliOption("--action")] string Action,
[property: CliOption("--firewall-policy")] string FirewallPolicy
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--dest-address-groups")]
    public string[]? DestAddressGroups { get; set; }

    [CliOption("--dest-fqdns")]
    public string[]? DestFqdns { get; set; }

    [CliOption("--dest-ip-ranges")]
    public string[]? DestIpRanges { get; set; }

    [CliOption("--dest-region-codes")]
    public string[]? DestRegionCodes { get; set; }

    [CliOption("--dest-threat-intelligence")]
    public string[]? DestThreatIntelligence { get; set; }

    [CliOption("--direction")]
    public string? Direction { get; set; }

    [CliOption("--[no-]disabled")]
    public string[]? NoDisabled { get; set; }

    [CliOption("--[no-]enable-logging")]
    public string[]? NoEnableLogging { get; set; }

    [CliOption("--layer4-configs")]
    public string[]? Layer4Configs { get; set; }

    [CliOption("--src-address-groups")]
    public string[]? SrcAddressGroups { get; set; }

    [CliOption("--src-fqdns")]
    public string[]? SrcFqdns { get; set; }

    [CliOption("--src-ip-ranges")]
    public string[]? SrcIpRanges { get; set; }

    [CliOption("--src-region-codes")]
    public string[]? SrcRegionCodes { get; set; }

    [CliOption("--src-secure-tags")]
    public string[]? SrcSecureTags { get; set; }

    [CliOption("--src-threat-intelligence")]
    public string[]? SrcThreatIntelligence { get; set; }

    [CliOption("--target-secure-tags")]
    public string[]? TargetSecureTags { get; set; }

    [CliOption("--target-service-accounts")]
    public string[]? TargetServiceAccounts { get; set; }

    [CliOption("--firewall-policy-region")]
    public string? FirewallPolicyRegion { get; set; }

    [CliFlag("--global-firewall-policy")]
    public bool? GlobalFirewallPolicy { get; set; }
}