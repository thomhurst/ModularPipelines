using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesPluginOptions : KubernetesOptions
{
    public KubernetesPluginOptions()
    {
        CommandParts = ["plugin"];
    }
}
