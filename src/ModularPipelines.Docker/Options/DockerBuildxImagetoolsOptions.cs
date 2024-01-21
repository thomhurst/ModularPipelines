using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx", "imagetools")]
[ExcludeFromCodeCoverage]
public record DockerBuildxImagetoolsOptions : DockerOptions
{
}
