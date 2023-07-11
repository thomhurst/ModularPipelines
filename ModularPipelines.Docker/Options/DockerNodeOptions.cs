using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("node")]
public record DockerNodeOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Command) : DockerOptions
{
}