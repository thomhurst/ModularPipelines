using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("buildx", "imagetools")]
[ExcludeFromCodeCoverage]
public record DockerBuildxImagetoolsOptions : DockerOptions
{
}
