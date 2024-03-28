using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesCreateServiceOptions : KubernetesOptions
{
    public KubernetesCreateServiceOptions()
    {
        CommandParts = ["create", "service"];
    }
}
