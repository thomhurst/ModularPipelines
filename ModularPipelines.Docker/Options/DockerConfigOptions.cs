using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("config")]
public record DockerConfigOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Command) : DockerOptions
{
}