using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesApiResourcesOptions : KubernetesOptions
{
    public KubernetesApiResourcesOptions()
    {
        CommandParts = ["api-resources"];
    }

    [CommandSwitch("--api-group")]
    public string? ApiGroup { get; set; }

    [BooleanCommandSwitch("--cached")]
    public bool? Cached { get; set; }

    [BooleanCommandSwitch("--namespaced")]
    public bool? Namespaced { get; set; }

    [BooleanCommandSwitch("--no-headers")]
    public bool? NoHeaders { get; set; }

    [CommandSwitch("--output")]
    public string? Output { get; set; }

    [CommandSwitch("--sort-by")]
    public string? SortBy { get; set; }

    [CommandSwitch("--verbs")]
    public IEnumerable<string>? Verbs { get; set; }
}
