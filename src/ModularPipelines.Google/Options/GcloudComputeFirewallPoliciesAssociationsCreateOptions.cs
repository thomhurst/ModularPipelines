using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "firewall-policies", "associations", "create")]
public record GcloudComputeFirewallPoliciesAssociationsCreateOptions(
[property: CommandSwitch("--firewall-policy")] string FirewallPolicy
) : GcloudOptions
{
    [CommandSwitch("--folder")]
    public string? Folder { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--organization")]
    public string? Organization { get; set; }

    [BooleanCommandSwitch("--replace-association-on-target")]
    public bool? ReplaceAssociationOnTarget { get; set; }
}