using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("top", "node")]
[ExcludeFromCodeCoverage]
public record KubernetesTopNodeOptions([property: CliArgument] string Name) : KubernetesOptions
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