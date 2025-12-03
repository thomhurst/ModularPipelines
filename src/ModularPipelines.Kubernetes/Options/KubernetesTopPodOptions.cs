using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("top", "pod")]
[ExcludeFromCodeCoverage]
public record KubernetesTopPodOptions([property: CliArgument] string Name) : KubernetesOptions
{
    [CliFlag("--all-namespaces")]
    public virtual bool? AllNamespaces { get; set; }

    [CliFlag("--containers")]
    public virtual bool? Containers { get; set; }

    [CliOption("--field-selector")]
    public string? FieldSelector { get; set; }

    [CliFlag("--no-headers")]
    public virtual bool? NoHeaders { get; set; }

    [CliOption("--selector")]
    public string? Selector { get; set; }

    [CliOption("--sort-by")]
    public string? SortBy { get; set; }

    [CliFlag("--use-protocol-buffers")]
    public virtual bool? UseProtocolBuffers { get; set; }
}