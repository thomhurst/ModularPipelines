using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("image tag")]
[ExcludeFromCodeCoverage]
public record DockerImageTagOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Sourceimage { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Targetimage { get; set; }
}