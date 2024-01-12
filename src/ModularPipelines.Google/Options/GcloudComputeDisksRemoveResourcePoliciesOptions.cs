using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "disks", "remove-resource-policies")]
public record GcloudComputeDisksRemoveResourcePoliciesOptions(
[property: PositionalArgument] string DiskName,
[property: CommandSwitch("--resource-policies")] string[] ResourcePolicies
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}