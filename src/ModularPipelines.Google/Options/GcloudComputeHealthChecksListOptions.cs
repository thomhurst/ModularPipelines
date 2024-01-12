using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "health-checks", "list")]
public record GcloudComputeHealthChecksListOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--protocol")]
    public string? Protocol { get; set; }

    [CommandSwitch("--regexp")]
    public string? Regexp { get; set; }

    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [CommandSwitch("--regions")]
    public string[]? Regions { get; set; }
}