using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("alpha")]
[ExcludeFromCodeCoverage]
public record KubernetesAlphaOptions : KubernetesOptions
{
    [CliOption("--api-group")]
    public string? ApiGroup { get; set; }

    [CliFlag("--cached")]
    public virtual bool? Cached { get; set; }

    [CliFlag("--namespaced")]
    public virtual bool? Namespaced { get; set; }

    [CliFlag("--no-headers")]
    public virtual bool? NoHeaders { get; set; }

    [CliOption("--output")]
    public string? Output { get; set; }

    [CliOption("--sort-by")]
    public string? SortBy { get; set; }

    [CliOption("--verbs")]
    public string[]? Verbs { get; set; }
}