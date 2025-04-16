using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx", "prune")]
[ExcludeFromCodeCoverage]
public record DockerBuildxPruneOptions : DockerOptions
{
    [BooleanCommandSwitch("--all")]
    public virtual bool? All { get; set; }

    [CommandSwitch("--filter")]
    public virtual string? Filter { get; set; }

    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }

    [CommandSwitch("--keep-storage")]
    public virtual string? KeepStorage { get; set; }

    [CommandSwitch("--verbose")]
    public virtual string? Verbose { get; set; }
}
