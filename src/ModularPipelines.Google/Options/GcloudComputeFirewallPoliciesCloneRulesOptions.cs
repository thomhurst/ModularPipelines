using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "firewall-policies", "clone-rules")]
public record GcloudComputeFirewallPoliciesCloneRulesOptions(
[property: PositionalArgument] string FirewallPolicy,
[property: CommandSwitch("--source-firewall-policy")] string SourceFirewallPolicy
) : GcloudOptions
{
    [CommandSwitch("--organization")]
    public string? Organization { get; set; }
}