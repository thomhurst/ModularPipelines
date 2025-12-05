using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("options")]
[ExcludeFromCodeCoverage]
public record KubernetesOptionsOptions : KubernetesOptions
{
    [CliFlag("--name-only")]
    public virtual bool? NameOnly { get; set; }
}