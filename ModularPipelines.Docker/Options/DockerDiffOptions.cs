using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("diff")]
public record DockerDiffOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Container) : DockerOptions
{
}