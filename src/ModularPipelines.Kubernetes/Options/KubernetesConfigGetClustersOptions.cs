using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("config", "get-clusters")]
[ExcludeFromCodeCoverage]
public record KubernetesConfigGetClustersOptions : KubernetesOptions
{
    [CliFlag("--no-headers")]
    public virtual bool? NoHeaders { get; set; }

    [CliOption("--output")]
    public virtual string? Output { get; set; }
}