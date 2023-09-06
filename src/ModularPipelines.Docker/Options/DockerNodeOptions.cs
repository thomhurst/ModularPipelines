using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("node")]
[ExcludeFromCodeCoverage]
public record DockerNodeOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Command) : DockerOptions
{
}
