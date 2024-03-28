using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesTopOptions : KubernetesOptions
{
    public KubernetesTopOptions()
    {
        CommandParts = ["top"];
    }
}
