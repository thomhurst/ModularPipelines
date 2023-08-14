using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("context use")]
public record DockerContextUseOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Context) : DockerOptions
{
}