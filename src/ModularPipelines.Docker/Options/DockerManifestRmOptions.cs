using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("manifest rm")]
public record DockerManifestRmOptions([property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> ManifestsList) : DockerOptions
{
}