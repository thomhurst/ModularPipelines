using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("swarm", "unlock")]
[ExcludeFromCodeCoverage]
public record DockerSwarmUnlockOptions : DockerOptions
{
}
