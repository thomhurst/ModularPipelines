using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesConfigOptions : KubernetesOptions
{
    public KubernetesConfigOptions()
    {
        CommandParts = ["config"];
    }

    [PositionalArgument(PlaceholderName = "<SUBCOMMAND>")]
    public string? SUBCOMMAND { get; set; }
}
