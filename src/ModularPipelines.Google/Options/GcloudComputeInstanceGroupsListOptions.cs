using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instance-groups", "list")]
public record GcloudComputeInstanceGroupsListOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--regexp")]
    public string? Regexp { get; set; }

    [BooleanCommandSwitch("--only-managed")]
    public bool? OnlyManaged { get; set; }

    [BooleanCommandSwitch("--only-unmanaged")]
    public bool? OnlyUnmanaged { get; set; }

    [CommandSwitch("--regions")]
    public string[]? Regions { get; set; }

    [CommandSwitch("--zones")]
    public string[]? Zones { get; set; }
}