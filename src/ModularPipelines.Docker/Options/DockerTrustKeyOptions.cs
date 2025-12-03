using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("trust", "key")]
[ExcludeFromCodeCoverage]
public record DockerTrustKeyOptions : DockerOptions
{
}
