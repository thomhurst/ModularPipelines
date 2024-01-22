using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("swarm", "unlock-key")]
[ExcludeFromCodeCoverage]
public record DockerSwarmUnlockKeyOptions : DockerOptions
{
    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [CommandSwitch("--rotate")]
    public string? Rotate { get; set; }
}
