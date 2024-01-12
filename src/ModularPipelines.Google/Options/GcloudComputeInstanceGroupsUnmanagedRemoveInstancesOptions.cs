using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instance-groups", "unmanaged", "remove-instances")]
public record GcloudComputeInstanceGroupsUnmanagedRemoveInstancesOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--instances")] string[] Instances
) : GcloudOptions
{
    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}