using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CliCommand("service", "scale")]
[ExcludeFromCodeCoverage]
public record DockerServiceScaleOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual IEnumerable<KeyValue>? ServiceReplicas { get; set; }

    [CliFlag("--detach")]
    public virtual bool? Detach { get; set; }
}
