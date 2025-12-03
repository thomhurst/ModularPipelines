using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "network-firewall-policies", "associations", "delete")]
public record GcloudComputeNetworkFirewallPoliciesAssociationsDeleteOptions(
[property: CliOption("--firewall-policy")] string FirewallPolicy,
[property: CliOption("--name")] string Name
) : GcloudOptions
{
    [CliOption("--firewall-policy-region")]
    public string? FirewallPolicyRegion { get; set; }

    [CliFlag("--global-firewall-policy")]
    public bool? GlobalFirewallPolicy { get; set; }
}