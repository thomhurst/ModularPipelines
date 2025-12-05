using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("system", "prune")]
[ExcludeFromCodeCoverage]
public record DockerSystemPruneOptions : DockerOptions
{
    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    [CliOption("--filter")]
    public virtual string? Filter { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliOption("--volumes")]
    public virtual string? Volumes { get; set; }
}
