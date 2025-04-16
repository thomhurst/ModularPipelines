using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("plugin")]
[ExcludeFromCodeCoverage]
public record KubernetesPluginOptions : KubernetesOptions
{
    [BooleanCommandSwitch("--name-only")]
    public virtual bool? NameOnly { get; set; }
}