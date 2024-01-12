using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "firewall-policies", "associations", "delete")]
public record GcloudComputeFirewallPoliciesAssociationsDeleteOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--firewall-policy")] string FirewallPolicy
) : GcloudOptions
{
    [CommandSwitch("--organization")]
    public string? Organization { get; set; }
}