using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "firewall-policies", "create")]
public record GcloudComputeFirewallPoliciesCreateOptions(
[property: CommandSwitch("--short-name")] string ShortName,
[property: CommandSwitch("--folder")] string Folder,
[property: CommandSwitch("--organization")] string Organization
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }
}