using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("plugin")]
[ExcludeFromCodeCoverage]
public record KubernetesPluginOptions : KubernetesOptions
{
    [CliFlag("--name-only")]
    public virtual bool? NameOnly { get; set; }
}