using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("pause")]
[ExcludeFromCodeCoverage]
public record DockerPauseOptions([property: PositionalArgument(Position = Position.AfterSwitches)] IEnumerable<string> Containers) : DockerOptions
{
}
