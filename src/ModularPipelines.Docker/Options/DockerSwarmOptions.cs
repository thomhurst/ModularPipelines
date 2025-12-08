using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliSubCommand("swarm")]
[ExcludeFromCodeCoverage]
public record DockerSwarmOptions : DockerOptions
{
}
