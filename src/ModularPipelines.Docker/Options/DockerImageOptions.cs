using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("image")]
[ExcludeFromCodeCoverage]
public record DockerImageOptions : DockerOptions
{
}
