using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesConfigDeleteContextOptions : KubernetesOptions
{
    public KubernetesConfigDeleteContextOptions()
    {
        CommandParts = ["config", "delete-context"];
    }

    [PositionalArgument(PlaceholderName = "<NAME>")]
    public string? NAME { get; set; }
}
