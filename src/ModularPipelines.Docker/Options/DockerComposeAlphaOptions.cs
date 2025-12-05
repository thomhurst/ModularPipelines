using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("compose", "alpha")]
[ExcludeFromCodeCoverage]
public record DockerComposeAlphaOptions : DockerOptions
{
}
