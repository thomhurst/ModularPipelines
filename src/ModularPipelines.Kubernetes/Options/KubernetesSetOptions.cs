using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesSetOptions : KubernetesOptions
{
    public KubernetesSetOptions()
    {
        CommandParts = ["set"];
    }

    [PositionalArgument(PlaceholderName = "<SUBCOMMAND>")]
    public string? SUBCOMMAND { get; set; }
}
