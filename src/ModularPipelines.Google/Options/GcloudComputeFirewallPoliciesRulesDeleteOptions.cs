using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "firewall-policies", "rules", "delete")]
public record GcloudComputeFirewallPoliciesRulesDeleteOptions(
[property: PositionalArgument] string Priority,
[property: CommandSwitch("--firewall-policy")] string FirewallPolicy
) : GcloudOptions
{
    [CommandSwitch("--organization")]
    public string? Organization { get; set; }
}