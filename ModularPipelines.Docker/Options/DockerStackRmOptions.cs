using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("stack rm")]
public record DockerStackRmOptions([property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> Stacks) : DockerOptions
{
}