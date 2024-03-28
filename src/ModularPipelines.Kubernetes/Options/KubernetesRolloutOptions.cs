using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesRolloutOptions : KubernetesOptions
{
    public KubernetesRolloutOptions()
    {
        CommandParts = ["rollout"];
    }

    [PositionalArgument(PlaceholderName = "<SUBCOMMAND>")]
    public string? SUBCOMMAND { get; set; }
}
