using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("manifest rm")]
[ExcludeFromCodeCoverage]
public record DockerManifestRmOptions([property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> ManifestsList) : DockerOptions
{
}
