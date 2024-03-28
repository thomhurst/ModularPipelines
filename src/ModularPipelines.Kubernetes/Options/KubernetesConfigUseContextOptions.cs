using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesConfigUseContextOptions : KubernetesOptions
{
    public KubernetesConfigUseContextOptions()
    {
        CommandParts = ["config", "use-context"];
    }
}
