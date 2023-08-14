using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container")]
public record DockerContainerOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Command) : DockerOptions
{
}