using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesPluginListOptions : KubernetesOptions
{
    public KubernetesPluginListOptions()
    {
        CommandParts = ["plugin", "list"];
    }

    [BooleanCommandSwitch("--name-only")]
    public bool? NameOnly { get; set; }
}
