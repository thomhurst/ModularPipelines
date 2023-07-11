using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("image tag")]
public record DockerImageTagOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public string Sourceimage { get; set; }
    [PositionalArgument(Position = Position.AfterArguments)]
    public string Targetimage { get; set; }
}