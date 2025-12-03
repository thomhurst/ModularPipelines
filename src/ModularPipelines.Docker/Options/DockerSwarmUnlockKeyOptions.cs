using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("swarm", "unlock-key")]
[ExcludeFromCodeCoverage]
public record DockerSwarmUnlockKeyOptions : DockerOptions
{
    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliOption("--rotate")]
    public virtual string? Rotate { get; set; }
}
