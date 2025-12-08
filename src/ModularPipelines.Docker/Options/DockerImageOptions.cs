using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliSubCommand("image")]
[ExcludeFromCodeCoverage]
public record DockerImageOptions : DockerOptions
{
}
