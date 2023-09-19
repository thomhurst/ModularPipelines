using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("image")]
[ExcludeFromCodeCoverage]
public record DockerImageOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Command) : DockerOptions
{
}
