using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CliCommand("compose", "alpha", "scale")]
[ExcludeFromCodeCoverage]
public record DockerComposeAlphaScaleOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual IEnumerable<KeyValue>? ServiceReplicas { get; set; }

    [CliFlag("--no-deps")]
    public virtual bool? NoDeps { get; set; }
}
