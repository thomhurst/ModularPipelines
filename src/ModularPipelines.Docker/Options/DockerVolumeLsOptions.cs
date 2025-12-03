using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("volume", "ls")]
[ExcludeFromCodeCoverage]
public record DockerVolumeLsOptions : DockerOptions
{
    [CliOption("--cluster")]
    public virtual string? Cluster { get; set; }

    [CliOption("--filter")]
    public virtual string? Filter { get; set; }

    [CliOption("--format")]
    public virtual string? Format { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }
}
