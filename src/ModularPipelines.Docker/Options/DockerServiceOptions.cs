using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("service")]
public record DockerServiceOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Command) : DockerOptions
{
}