using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("image", "prune")]
[ExcludeFromCodeCoverage]
public record DockerImagePruneOptions : DockerOptions
{
    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    [CliOption("--filter")]
    public virtual string? Filter { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }
}
