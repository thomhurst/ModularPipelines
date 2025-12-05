using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("swarm")]
[ExcludeFromCodeCoverage]
public record DockerSwarmOptions : DockerOptions
{
}
