using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "firewall-policies", "update")]
public record GcloudComputeFirewallPoliciesUpdateOptions(
[property: PositionalArgument] string FirewallPolicy
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--organization")]
    public string? Organization { get; set; }
}