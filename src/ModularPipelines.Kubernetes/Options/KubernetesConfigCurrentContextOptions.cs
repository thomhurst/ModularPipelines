using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("config", "current-context")]
[ExcludeFromCodeCoverage]
public record KubernetesConfigCurrentContextOptions : KubernetesOptions
{
    [CliFlag("--no-headers")]
    public virtual bool? NoHeaders { get; set; }

    [CliOption("--output")]
    public virtual string? Output { get; set; }
}