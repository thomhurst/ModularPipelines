using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "network-firewall-policies", "rules", "describe")]
public record GcloudComputeNetworkFirewallPoliciesRulesDescribeOptions(
[property: CliArgument] string Priority,
[property: CliOption("--firewall-policy")] string FirewallPolicy
) : GcloudOptions
{
    [CliOption("--firewall-policy-region")]
    public string? FirewallPolicyRegion { get; set; }

    [CliFlag("--global-firewall-policy")]
    public bool? GlobalFirewallPolicy { get; set; }
}