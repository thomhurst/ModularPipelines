using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesExplainOptions : KubernetesOptions
{
    public KubernetesExplainOptions()
    {
        CommandParts = ["explain"];
    }

    [CommandSwitch("--api-version")]
    public string? ApiVersion { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public bool? Recursive { get; set; }

    [PositionalArgument(PlaceholderName = "<RESOURCE>")]
    public string? RESOURCE { get; set; }
}
