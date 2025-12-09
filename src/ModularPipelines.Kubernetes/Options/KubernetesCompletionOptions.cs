using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliSubCommand("completion")]
[ExcludeFromCodeCoverage]
public record KubernetesCompletionOptions([property: CliArgument] string Shell) : KubernetesOptions
{
    [CliFlag("--no-headers")]
    public virtual bool? NoHeaders { get; set; }

    [CliOption("--output")]
    public virtual string? Output { get; set; }
}