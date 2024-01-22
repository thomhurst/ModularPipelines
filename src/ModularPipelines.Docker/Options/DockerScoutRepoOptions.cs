using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout", "repo")]
[ExcludeFromCodeCoverage]
public record DockerScoutRepoOptions : DockerOptions
{
}
