using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("image")]
public record DockerImageOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Command) : DockerOptions
{
}