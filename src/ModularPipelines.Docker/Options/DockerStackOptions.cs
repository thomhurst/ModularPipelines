using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("stack")]
public record DockerStackOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Command) : DockerOptions
{
}