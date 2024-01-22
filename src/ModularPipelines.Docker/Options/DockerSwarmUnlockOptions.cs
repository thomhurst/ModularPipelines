using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("swarm", "unlock")]
[ExcludeFromCodeCoverage]
public record DockerSwarmUnlockOptions : DockerOptions
{
}
