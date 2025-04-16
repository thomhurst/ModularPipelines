using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("plugin", "list")]
[ExcludeFromCodeCoverage]
public record KubernetesPluginListOptions : KubernetesOptions
{
    [BooleanCommandSwitch("--name-only")]
    public virtual bool? NameOnly { get; set; }
}