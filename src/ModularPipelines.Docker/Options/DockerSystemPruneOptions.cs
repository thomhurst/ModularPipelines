using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("system", "prune")]
[ExcludeFromCodeCoverage]
public record DockerSystemPruneOptions : DockerOptions
{
    [BooleanCommandSwitch("--all")]
    public virtual bool? All { get; set; }

    [CommandSwitch("--filter")]
    public virtual string? Filter { get; set; }

    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }

    [CommandSwitch("--volumes")]
    public virtual string? Volumes { get; set; }
}
