using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("context")]
public record DockerContextOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Command) : DockerOptions
{
}