using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("swarm")]
[ExcludeFromCodeCoverage]
public record DockerSwarmOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Command) : DockerOptions;