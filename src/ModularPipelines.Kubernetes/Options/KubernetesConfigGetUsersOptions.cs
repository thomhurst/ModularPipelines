using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesConfigGetUsersOptions : KubernetesOptions
{
    public KubernetesConfigGetUsersOptions()
    {
        CommandParts = ["config", "get-users"];
    }
}
