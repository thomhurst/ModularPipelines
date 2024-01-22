using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx", "imagetools")]
[ExcludeFromCodeCoverage]
public record DockerBuildxImagetoolsOptions : DockerOptions
{
}
