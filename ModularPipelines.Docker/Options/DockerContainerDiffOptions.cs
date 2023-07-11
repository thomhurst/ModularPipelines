using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container diff")]
public record DockerContainerDiffOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Container) : DockerOptions
{
}