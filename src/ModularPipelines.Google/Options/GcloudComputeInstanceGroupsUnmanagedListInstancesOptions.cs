using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instance-groups", "unmanaged", "list-instances")]
public record GcloudComputeInstanceGroupsUnmanagedListInstancesOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--regexp")]
    public string? Regexp { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}