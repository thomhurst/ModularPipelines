using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "firewall-policies", "rules", "create")]
public record GcloudComputeFirewallPoliciesRulesCreateOptions(
[property: PositionalArgument] string Priority,
[property: CommandSwitch("--action")] string Action,
[property: CommandSwitch("--firewall-policy")] string FirewallPolicy
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--dest-address-groups")]
    public string[]? DestAddressGroups { get; set; }

    [CommandSwitch("--dest-fqdns")]
    public string[]? DestFqdns { get; set; }

    [CommandSwitch("--dest-ip-ranges")]
    public string[]? DestIpRanges { get; set; }

    [CommandSwitch("--dest-region-codes")]
    public string[]? DestRegionCodes { get; set; }

    [CommandSwitch("--dest-threat-intelligence")]
    public string[]? DestThreatIntelligence { get; set; }

    [CommandSwitch("--direction")]
    public string? Direction { get; set; }

    [CommandSwitch("--[no-]disabled")]
    public string[]? NoDisabled { get; set; }

    [CommandSwitch("--[no-]enable-logging")]
    public string[]? NoEnableLogging { get; set; }

    [CommandSwitch("--layer4-configs")]
    public string[]? Layer4Configs { get; set; }

    [CommandSwitch("--organization")]
    public string? Organization { get; set; }

    [CommandSwitch("--src-address-groups")]
    public string[]? SrcAddressGroups { get; set; }

    [CommandSwitch("--src-fqdns")]
    public string[]? SrcFqdns { get; set; }

    [CommandSwitch("--src-ip-ranges")]
    public string[]? SrcIpRanges { get; set; }

    [CommandSwitch("--src-region-codes")]
    public string[]? SrcRegionCodes { get; set; }

    [CommandSwitch("--src-threat-intelligence")]
    public string[]? SrcThreatIntelligence { get; set; }

    [CommandSwitch("--target-resources")]
    public string[]? TargetResources { get; set; }

    [CommandSwitch("--target-service-accounts")]
    public string[]? TargetServiceAccounts { get; set; }
}