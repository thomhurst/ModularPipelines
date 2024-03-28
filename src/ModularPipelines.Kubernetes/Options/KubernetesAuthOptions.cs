using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesAuthOptions : KubernetesOptions
{
    public KubernetesAuthOptions()
    {
        CommandParts = ["auth"];
    }
}
