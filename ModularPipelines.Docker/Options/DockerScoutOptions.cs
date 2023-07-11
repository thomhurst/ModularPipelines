using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout")]
public record DockerScoutOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Command) : DockerOptions
{
}