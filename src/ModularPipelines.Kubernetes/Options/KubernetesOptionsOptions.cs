using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("options")]
[ExcludeFromCodeCoverage]
public record KubernetesOptionsOptions : KubernetesOptions
{
    [BooleanCommandSwitch("--name-only")]
    public virtual bool? NameOnly { get; set; }
}