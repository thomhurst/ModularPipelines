using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container", "ls")]
[ExcludeFromCodeCoverage]
public record DockerContainerLsOptions : DockerOptions
{
    [BooleanCommandSwitch("--all")]
    public virtual bool? All { get; set; }

    [CommandSwitch("--filter")]
    public virtual string? Filter { get; set; }

    [CommandSwitch("--format")]
    public virtual string? Format { get; set; }

    [CommandSwitch("--last")]
    public virtual int? Last { get; set; }

    [CommandSwitch("--latest")]
    public virtual string? Latest { get; set; }

    [BooleanCommandSwitch("--no-trunc")]
    public virtual bool? NoTrunc { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CommandSwitch("--size")]
    public virtual string? Size { get; set; }
}
