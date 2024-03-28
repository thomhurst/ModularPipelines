using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesConfigCurrentContextOptions : KubernetesOptions
{
    public KubernetesConfigCurrentContextOptions()
    {
        CommandParts = ["config", "current-context"];
    }
}
