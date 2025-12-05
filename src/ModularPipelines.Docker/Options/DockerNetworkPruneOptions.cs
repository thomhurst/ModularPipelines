using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("network", "prune")]
[ExcludeFromCodeCoverage]
public record DockerNetworkPruneOptions : DockerOptions
{
    [CliOption("--filter")]
    public virtual string? Filter { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }
}
