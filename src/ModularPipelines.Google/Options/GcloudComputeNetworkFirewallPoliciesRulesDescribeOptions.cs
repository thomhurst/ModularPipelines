using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "network-firewall-policies", "rules", "describe")]
public record GcloudComputeNetworkFirewallPoliciesRulesDescribeOptions(
[property: PositionalArgument] string Priority,
[property: CommandSwitch("--firewall-policy")] string FirewallPolicy
) : GcloudOptions
{
    [CommandSwitch("--firewall-policy-region")]
    public string? FirewallPolicyRegion { get; set; }

    [BooleanCommandSwitch("--global-firewall-policy")]
    public bool? GlobalFirewallPolicy { get; set; }
}