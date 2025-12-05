using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("container", "prune")]
[ExcludeFromCodeCoverage]
public record DockerContainerPruneOptions : DockerOptions
{
    [CliOption("--filter")]
    public virtual string? Filter { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }
}
