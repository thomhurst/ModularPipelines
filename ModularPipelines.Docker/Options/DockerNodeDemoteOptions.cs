using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("node demote")]
public record DockerNodeDemoteOptions([property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> Node) : DockerOptions
{
}