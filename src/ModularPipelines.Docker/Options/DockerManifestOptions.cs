using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("manifest")]
[ExcludeFromCodeCoverage]
public record DockerManifestOptions : DockerOptions
{
}
