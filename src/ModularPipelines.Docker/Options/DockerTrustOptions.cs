using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("trust")]
[ExcludeFromCodeCoverage]
public record DockerTrustOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Command) : DockerOptions
{
}
