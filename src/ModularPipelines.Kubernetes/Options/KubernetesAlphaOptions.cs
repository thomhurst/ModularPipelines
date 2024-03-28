using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesAlphaOptions : KubernetesOptions
{
    public KubernetesAlphaOptions()
    {
        CommandParts = ["alpha"];
    }
}
