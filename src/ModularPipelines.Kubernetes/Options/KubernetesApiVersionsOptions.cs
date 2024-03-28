using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesApiVersionsOptions : KubernetesOptions
{
    public KubernetesApiVersionsOptions()
    {
        CommandParts = ["api-versions"];
    }
}
