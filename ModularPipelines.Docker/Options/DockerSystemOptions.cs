using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("system")]
public record DockerSystemOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Command) : DockerOptions
{
}