using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("top")]
[ExcludeFromCodeCoverage]
public record KubernetesTopOptions : KubernetesOptions
{
    [CliFlag("--no-headers")]
    public virtual bool? NoHeaders { get; set; }

    [CliOption("--selector")]
    public virtual string? Selector { get; set; }

    [CliOption("--sort-by")]
    public virtual string? SortBy { get; set; }

    [CliFlag("--use-protocol-buffers")]
    public virtual bool? UseProtocolBuffers { get; set; }
}