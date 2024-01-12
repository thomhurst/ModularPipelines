using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "firewall-policies", "list")]
public record GcloudComputeFirewallPoliciesListOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--folder")] string Folder,
[property: CommandSwitch("--organization")] string Organization
) : GcloudOptions
{
    [CommandSwitch("--regexp")]
    public string? Regexp { get; set; }
}