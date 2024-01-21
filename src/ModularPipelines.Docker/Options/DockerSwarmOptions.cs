using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("swarm")]
[ExcludeFromCodeCoverage]
public record DockerSwarmOptions : DockerOptions
{
}
