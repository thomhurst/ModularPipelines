using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("builder")]
public record DockerBuilderOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Command) : DockerOptions
{
}