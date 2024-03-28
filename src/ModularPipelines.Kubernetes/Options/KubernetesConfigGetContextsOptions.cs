using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesConfigGetContextsOptions : KubernetesOptions
{
    public KubernetesConfigGetContextsOptions(
        string output
)
    {
        CommandParts = ["config", "get-contexts"];
        Output = output;
    }

    [BooleanCommandSwitch("--no-headers")]
    public bool? NoHeaders { get; set; }

    [CommandSwitch("--output")]
    public string? Output { get; set; }
}
