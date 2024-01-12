using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "firewall-policies", "list-rules")]
public record GcloudComputeFirewallPoliciesListRulesOptions(
[property: PositionalArgument] string FirewallPolicy,
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--organization")]
    public string? Organization { get; set; }

    [CommandSwitch("--regexp")]
    public string? Regexp { get; set; }
}