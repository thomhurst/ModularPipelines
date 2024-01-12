using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instance-groups", "managed", "list")]
public record GcloudComputeInstanceGroupsManagedListOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--regexp")]
    public string? Regexp { get; set; }

    [CommandSwitch("--regions")]
    public string[]? Regions { get; set; }

    [CommandSwitch("--zones")]
    public string[]? Zones { get; set; }
}