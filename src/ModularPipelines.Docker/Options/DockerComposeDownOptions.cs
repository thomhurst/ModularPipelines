using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose", "down")]
[ExcludeFromCodeCoverage]
public record DockerComposeDownOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Services { get; set; }

    [BooleanCommandSwitch("--remove-orphans")]
    public virtual bool? RemoveOrphans { get; set; }

    [BooleanCommandSwitch("--rmi")]
    public virtual bool? Rmi { get; set; }

    [CommandSwitch("--timeout")]
    public virtual string? Timeout { get; set; }

    [CommandSwitch("--volumes")]
    public virtual string? Volumes { get; set; }
}
