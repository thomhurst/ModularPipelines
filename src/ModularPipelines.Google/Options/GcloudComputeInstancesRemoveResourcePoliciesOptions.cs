using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instances", "remove-resource-policies")]
public record GcloudComputeInstancesRemoveResourcePoliciesOptions(
[property: PositionalArgument] string InstanceName,
[property: CommandSwitch("--resource-policies")] string[] ResourcePolicies
) : GcloudOptions
{
    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}