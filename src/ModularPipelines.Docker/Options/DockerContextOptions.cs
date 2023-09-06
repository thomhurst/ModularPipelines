using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("context")]
[ExcludeFromCodeCoverage]
public record DockerContextOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Command) : DockerOptions
{
}
