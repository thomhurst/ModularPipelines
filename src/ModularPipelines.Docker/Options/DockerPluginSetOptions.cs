using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("plugin set")]
[ExcludeFromCodeCoverage]
public record DockerPluginSetOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Plugin, [property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> KeyValues) : DockerOptions
{
}
