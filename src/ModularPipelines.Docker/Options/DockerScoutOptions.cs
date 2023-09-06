using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout")]
[ExcludeFromCodeCoverage]
public record DockerScoutOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Command) : DockerOptions
{
}
