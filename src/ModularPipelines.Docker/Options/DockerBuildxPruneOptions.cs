using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("buildx", "prune")]
[ExcludeFromCodeCoverage]
public record DockerBuildxPruneOptions : DockerOptions
{
    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    [CliOption("--filter")]
    public virtual string? Filter { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliOption("--keep-storage")]
    public virtual string? KeepStorage { get; set; }

    [CliOption("--verbose")]
    public virtual string? Verbose { get; set; }
}
