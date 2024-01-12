using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "network-firewall-policies", "get-effective-firewalls")]
public record GcloudComputeNetworkFirewallPoliciesGetEffectiveFirewallsOptions(
[property: CommandSwitch("--network")] string Network
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}