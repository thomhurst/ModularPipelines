using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("completion")]
[ExcludeFromCodeCoverage]
public record KubernetesCompletionOptions([property: CliArgument] string Shell) : KubernetesOptions
{
    [CliFlag("--no-headers")]
    public virtual bool? NoHeaders { get; set; }

    [CliOption("--output")]
    public string? Output { get; set; }
}