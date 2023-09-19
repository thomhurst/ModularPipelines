using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("service")]
[ExcludeFromCodeCoverage]
public record DockerServiceOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Command) : DockerOptions
{
}
