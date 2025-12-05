using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("scout", "repo")]
[ExcludeFromCodeCoverage]
public record DockerScoutRepoOptions : DockerOptions
{
}
