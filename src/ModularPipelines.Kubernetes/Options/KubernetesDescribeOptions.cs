using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("describe")]
[ExcludeFromCodeCoverage]
public record KubernetesDescribeOptions : KubernetesOptions
{
    [CliFlag("--all-namespaces")]
    public virtual bool? AllNamespaces { get; set; }

    [CliOption("--chunk-size")]
    public int? ChunkSize { get; set; }

    [CliOption("--filename")]
    public string[]? Filename { get; set; }

    [CliOption("--kustomize")]
    public string? Kustomize { get; set; }

    [CliFlag("--recursive")]
    public virtual bool? Recursive { get; set; }

    [CliOption("--selector")]
    public string? Selector { get; set; }

    [CliFlag("--show-events")]
    public virtual bool? ShowEvents { get; set; }
}