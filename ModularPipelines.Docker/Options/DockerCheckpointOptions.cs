using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("checkpoint")]
public record DockerCheckpointOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Command) : DockerOptions
{
}