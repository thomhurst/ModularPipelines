using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx", "stop")]
[ExcludeFromCodeCoverage]
public record DockerBuildxStopOptions : DockerOptions
{
}
