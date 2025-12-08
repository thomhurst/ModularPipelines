using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliSubCommand("manifest")]
[ExcludeFromCodeCoverage]
public record DockerManifestOptions : DockerOptions
{
}
