using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesCompletionOptions : KubernetesOptions
{
    public KubernetesCompletionOptions()
    {
        CommandParts = ["completion"];
    }

    [PositionalArgument(PlaceholderName = "<SHELL>")]
    public string? SHELL { get; set; }
}
