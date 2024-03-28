using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesConfigDeleteUserOptions : KubernetesOptions
{
    public KubernetesConfigDeleteUserOptions()
    {
        CommandParts = ["config", "delete-user"];
    }

    [PositionalArgument(PlaceholderName = "<NAME>")]
    public string? NAME { get; set; }
}
