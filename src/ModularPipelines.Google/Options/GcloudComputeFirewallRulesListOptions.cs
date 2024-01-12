using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "firewall-rules", "list")]
public record GcloudComputeFirewallRulesListOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--regexp")]
    public string? Regexp { get; set; }
}