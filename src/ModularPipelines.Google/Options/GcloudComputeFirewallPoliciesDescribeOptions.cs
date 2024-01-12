using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "firewall-policies", "describe")]
public record GcloudComputeFirewallPoliciesDescribeOptions(
[property: PositionalArgument] string FirewallPolicy
) : GcloudOptions
{
    [CommandSwitch("--organization")]
    public string? Organization { get; set; }
}