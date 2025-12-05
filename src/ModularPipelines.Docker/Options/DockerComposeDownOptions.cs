using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("compose", "down")]
[ExcludeFromCodeCoverage]
public record DockerComposeDownOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? Services { get; set; }

    [CliFlag("--remove-orphans")]
    public virtual bool? RemoveOrphans { get; set; }

    [CliFlag("--rmi")]
    public virtual bool? Rmi { get; set; }

    [CliOption("--timeout")]
    public virtual string? Timeout { get; set; }

    [CliOption("--volumes")]
    public virtual string? Volumes { get; set; }
}
