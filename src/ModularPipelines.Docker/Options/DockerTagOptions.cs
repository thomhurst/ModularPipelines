using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("tag")]
[ExcludeFromCodeCoverage]
public record DockerTagOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Sourceimage { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Targetimage { get; set; }
}