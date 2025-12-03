using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("scout", "cache", "df")]
[ExcludeFromCodeCoverage]
public record DockerScoutCacheDfOptions : DockerOptions
{
}
