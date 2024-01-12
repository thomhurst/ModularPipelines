using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "network-firewall-policies", "associations", "delete")]
public record GcloudComputeNetworkFirewallPoliciesAssociationsDeleteOptions(
[property: CommandSwitch("--firewall-policy")] string FirewallPolicy,
[property: CommandSwitch("--name")] string Name
) : GcloudOptions
{
    [CommandSwitch("--firewall-policy-region")]
    public string? FirewallPolicyRegion { get; set; }

    [BooleanCommandSwitch("--global-firewall-policy")]
    public bool? GlobalFirewallPolicy { get; set; }
}