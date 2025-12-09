using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliSubCommand("version")]
[ExcludeFromCodeCoverage]
public record KubernetesVersionOptions : KubernetesOptions
{
    [CliFlag("--client")]
    public virtual bool? Client { get; set; }

    [CliOption("--output")]
    public virtual string? Output { get; set; }

    [CliFlag("--short")]
    public virtual bool? Short { get; set; }
}