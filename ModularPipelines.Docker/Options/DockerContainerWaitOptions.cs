using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container wait")]
public record DockerContainerWaitOptions([property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> Container) : DockerOptions
{
}