using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("swarm")]
public record DockerSwarmOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Command) : DockerOptions
{
}