using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("config rm")]
public record DockerConfigRmOptions([property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> ConfigNamesNames) : DockerOptions
{
}