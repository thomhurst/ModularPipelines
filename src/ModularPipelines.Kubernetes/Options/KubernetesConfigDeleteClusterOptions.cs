using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesConfigDeleteClusterOptions : KubernetesOptions
{
    public KubernetesConfigDeleteClusterOptions()
    {
        CommandParts = ["config", "delete-cluster"];
    }

    [PositionalArgument(PlaceholderName = "<NAME>")]
    public string? NAME { get; set; }
}
