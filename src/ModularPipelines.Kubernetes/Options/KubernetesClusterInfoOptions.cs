using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesClusterInfoOptions : KubernetesOptions
{
    public KubernetesClusterInfoOptions()
    {
        CommandParts = ["cluster-info"];
    }
}
