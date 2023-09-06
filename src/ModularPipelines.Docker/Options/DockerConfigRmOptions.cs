using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("config rm")]
[ExcludeFromCodeCoverage]
public record DockerConfigRmOptions([property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> ConfigNamesNames) : DockerOptions
{
}
