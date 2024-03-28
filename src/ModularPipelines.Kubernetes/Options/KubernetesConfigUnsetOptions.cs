using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesConfigUnsetOptions : KubernetesOptions
{
    public KubernetesConfigUnsetOptions()
    {
        CommandParts = ["config", "unset"];
    }
}
