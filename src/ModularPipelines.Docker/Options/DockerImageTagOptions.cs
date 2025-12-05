using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("image", "tag")]
[ExcludeFromCodeCoverage]
public record DockerImageTagOptions : DockerOptions
{
}
