using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("checkpoint")]
[ExcludeFromCodeCoverage]
public record DockerCheckpointOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Command) : DockerOptions
{
}