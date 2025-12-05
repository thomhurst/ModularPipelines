using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("scout", "cache")]
[ExcludeFromCodeCoverage]
public record DockerScoutCacheOptions : DockerOptions
{
}
