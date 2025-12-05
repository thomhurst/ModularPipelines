using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("compose", "ls")]
[ExcludeFromCodeCoverage]
public record DockerComposeLsOptions : DockerOptions
{
    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    [CliOption("--filter")]
    public virtual string? Filter { get; set; }

    [CliOption("--format")]
    public virtual string? Format { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }
}
