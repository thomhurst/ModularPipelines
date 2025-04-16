using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx", "ls")]
[ExcludeFromCodeCoverage]
public record DockerBuildxLsOptions : DockerOptions
{
    [CommandSwitch("--builder")]
    public virtual string? Builder { get; set; }

    [CommandSwitch("--format")]
    public virtual string? Format { get; set; }
}
