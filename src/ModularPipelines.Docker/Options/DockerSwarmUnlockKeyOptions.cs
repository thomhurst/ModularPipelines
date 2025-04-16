using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("swarm", "unlock-key")]
[ExcludeFromCodeCoverage]
public record DockerSwarmUnlockKeyOptions : DockerOptions
{
    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CommandSwitch("--rotate")]
    public virtual string? Rotate { get; set; }
}
