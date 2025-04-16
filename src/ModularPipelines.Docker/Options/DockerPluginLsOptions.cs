using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("plugin", "ls")]
[ExcludeFromCodeCoverage]
public record DockerPluginLsOptions : DockerOptions
{
    [CommandSwitch("--filter")]
    public virtual string? Filter { get; set; }

    [CommandSwitch("--format")]
    public virtual string? Format { get; set; }

    [BooleanCommandSwitch("--no-trunc")]
    public virtual bool? NoTrunc { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }
}
