using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesCreateSecretOptions : KubernetesOptions
{
    public KubernetesCreateSecretOptions()
    {
        CommandParts = ["create", "secret"];
    }
}
