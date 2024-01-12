using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "network-firewall-policies", "clone-rules")]
public record GcloudComputeNetworkFirewallPoliciesCloneRulesOptions(
[property: PositionalArgument] string FirewallPolicy,
[property: CommandSwitch("--source-firewall-policy")] string SourceFirewallPolicy
) : GcloudOptions
{
    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}