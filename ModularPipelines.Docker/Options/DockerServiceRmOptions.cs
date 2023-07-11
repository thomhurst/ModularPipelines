using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("service rm")]
public record DockerServiceRmOptions([property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> Service) : DockerOptions
{
}