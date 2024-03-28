using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesConfigRenameContextOptions : KubernetesOptions
{
    public KubernetesConfigRenameContextOptions()
    {
        CommandParts = ["config", "rename-context"];
    }
}
