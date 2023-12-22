using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx use")]
[ExcludeFromCodeCoverage]
public record DockerBuildxUseOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Name) : DockerOptions
{
    [CommandSwitch("--default")]
    public string? Default { get; set; }

    [CommandSwitch("--global")]
    public string? Global { get; set; }

    [CommandSwitch("--builder")]
    public string? Builder { get; set; }
}