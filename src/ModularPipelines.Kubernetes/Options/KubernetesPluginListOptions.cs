using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("plugin", "list")]
[ExcludeFromCodeCoverage]
public record KubernetesPluginListOptions : KubernetesOptions
{
    [CliFlag("--name-only")]
    public virtual bool? NameOnly { get; set; }
}