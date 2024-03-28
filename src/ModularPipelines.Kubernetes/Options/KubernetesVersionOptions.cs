using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesVersionOptions : KubernetesOptions
{
    public KubernetesVersionOptions()
    {
        CommandParts = ["version"];
    }
}
