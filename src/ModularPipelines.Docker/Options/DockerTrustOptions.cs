using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("trust")]
[ExcludeFromCodeCoverage]
public record DockerTrustOptions : DockerOptions
{
}
