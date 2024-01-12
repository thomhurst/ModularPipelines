using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "network-firewall-policies", "associations", "create")]
public record GcloudComputeNetworkFirewallPoliciesAssociationsCreateOptions(
[property: CommandSwitch("--firewall-policy")] string FirewallPolicy,
[property: CommandSwitch("--network")] string Network
) : GcloudOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--replace-association-on-target")]
    public bool? ReplaceAssociationOnTarget { get; set; }

    [CommandSwitch("--firewall-policy-region")]
    public string? FirewallPolicyRegion { get; set; }

    [BooleanCommandSwitch("--global-firewall-policy")]
    public bool? GlobalFirewallPolicy { get; set; }
}