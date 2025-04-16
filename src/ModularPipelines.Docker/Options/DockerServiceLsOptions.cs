using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("service", "ls")]
[ExcludeFromCodeCoverage]
public record DockerServiceLsOptions : DockerOptions
{
    [CommandSwitch("--filter")]
    public virtual string? Filter { get; set; }

    [CommandSwitch("--format")]
    public virtual string? Format { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }
}
