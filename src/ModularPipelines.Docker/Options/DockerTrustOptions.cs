using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliSubCommand("trust")]
[ExcludeFromCodeCoverage]
public record DockerTrustOptions : DockerOptions
{
}
