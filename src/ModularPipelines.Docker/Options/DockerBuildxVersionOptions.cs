using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx", "version")]
[ExcludeFromCodeCoverage]
public record DockerBuildxVersionOptions : DockerOptions
{
    [CommandSwitch("--builder")]
    public virtual string? Builder { get; set; }
}
