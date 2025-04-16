using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx")]
[ExcludeFromCodeCoverage]
public record DockerBuildxOptions : DockerOptions
{
    [CommandSwitch("--builder")]
    public virtual string? Builder { get; set; }
}
