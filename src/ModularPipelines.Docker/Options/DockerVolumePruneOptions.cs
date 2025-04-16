using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("volume", "prune")]
[ExcludeFromCodeCoverage]
public record DockerVolumePruneOptions : DockerOptions
{
    [BooleanCommandSwitch("--all")]
    public virtual bool? All { get; set; }

    [CommandSwitch("--filter")]
    public virtual string? Filter { get; set; }

    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }
}
