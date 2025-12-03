using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("scout", "cache", "prune")]
[ExcludeFromCodeCoverage]
public record DockerScoutCachePruneOptions : DockerOptions
{
    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliOption("--sboms")]
    public virtual string? Sboms { get; set; }
}
