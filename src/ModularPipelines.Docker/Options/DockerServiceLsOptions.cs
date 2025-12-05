using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("service", "ls")]
[ExcludeFromCodeCoverage]
public record DockerServiceLsOptions : DockerOptions
{
    [CliOption("--filter")]
    public virtual string? Filter { get; set; }

    [CliOption("--format")]
    public virtual string? Format { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }
}
