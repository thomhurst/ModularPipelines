using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesConfigGetClustersOptions : KubernetesOptions
{
    public KubernetesConfigGetClustersOptions()
    {
        CommandParts = ["config", "get-clusters"];
    }
}
