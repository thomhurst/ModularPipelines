using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("swarm", "unlock")]
[ExcludeFromCodeCoverage]
public record DockerSwarmUnlockOptions : DockerOptions
{
}
