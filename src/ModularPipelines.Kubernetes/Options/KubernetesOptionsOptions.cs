using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesOptionsOptions : KubernetesOptions
{
    public KubernetesOptionsOptions()
    {
        CommandParts = ["options"];
    }
}
