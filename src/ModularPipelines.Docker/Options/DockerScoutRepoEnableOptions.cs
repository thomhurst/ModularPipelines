using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout repo enable")]
public record DockerScoutRepoEnableOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Repository) : DockerOptions
{
}