using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("swarm")]
[ExcludeFromCodeCoverage]
public record DockerSwarmOptions : DockerOptions
{
}
